using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using Verse;

namespace EdB.ModOrder
{
	/*
	 * The ModController class manages how modded user interface elements are injected into
	 * the game.  User interface elements are generally stored in the Verse.WindowStack class.
	 * The general approach that the mod takes is to keep an eye on the user-interface element
	 * at the top of that stack.  When it sees the element that it wants to replace, it replaces it.
	 * It derives from Unity Engine's MonoBehavior class (http://docs.unity3d.com/ScriptReference/MonoBehaviour.html)
	 * so that it can take advantage of its built-in event handling.  By implementing the Start(),
	 * OnLevelLoaded() and Update() methods, the mod can run custom logic immediately after all
	 * mods have been loaded, when we switch between the main menus and gameplay and every frame,
	 * respectively.
	 */
    class ModController : UnityEngine.MonoBehaviour
    {
		// This matches the name of the mod defined in Resources/About/About.xml.
		public static readonly string ModName = "EdB Mod Order";

		// The name we're using to store this controller class as a GameObject in the Unity engine.
		public static readonly string GameObjectName = "EdBModOrderController";

		// We keep track of top-most layer in the UI so that we can detect when it changes.
		protected Window currentWindow = null;

		// We keep track of whether we're in the middle of gameplay or if we're in the
		// game's main menus.
		protected bool gameplay = false;

		// The Start() method is called when the MonoBehavior first starts up.  This will
		// happen immediately after all mods are loaded.
		public virtual void Start()
		{
			// IMPORTANT: setting whether or not the MonoBehavior is enabled is very important.
			// If it's enabled, then the Update() method will get called every frame.  If it
			// is not enabled, then the Update() method will not get called every frame.  To
			// minimize impact on the game's performance, you should try to keep your MonoBehavior
			// disabled, if possible.  This is especially true during gameplay.  It's less
			// critical when in the game's menus where there's less performance stress, and
			// this mod enables the behavior when in the menus.
			this.Enabled = true;

			// Reload any textures that the mod uses
			ResetTextures();
		}

		// Define an accessor in case we want to insert any additional logic when enabling
		// or disabling the controller.  The enabled property itself is in the base class.
		protected bool Enabled {
			get {
				return this.enabled;
			}
			set {
				this.enabled = true;
			}
		}

		// OnLevelLoaded is called whenever the game switches from the main menu (level 0)
		// into gameplay (level 1).  This mod doesn't do anything during gameplay, so that
		// is a good time to disable the mod.  On the other hand, it needs to be enabled
		// when the main menus load.
		public void OnLevelWasLoaded(int level)
		{
			// Level 0 means we're in the game menus.
			if (level == 0) {
				this.gameplay = false;

				// Enable the behavior so that we start calling the Update() method every frame.
				this.Enabled = true;
			}
			// Level 1 means we're in gameplay.
			else if (level == 1) {
				this.gameplay = true;

				// Disable this behavior so that we stop calling the Update() method every frame.
				// Our mod doesn't do anything during gameplay, so there's no reason to keep it
				// enabled.
				this.Enabled = false;
			}
		}

		// Called every frame when the mod is enabled.
		public virtual void Update()
		{
			// We route the logic here based on whether or not we're in gameplay.
			// This particular mod doesn't do anything during gameplay, so we
			// don't really need this extra routing, but I left it in as an example.
			try {
				if (!gameplay) {
					MenusUpdate();
				}
				else {
					GameplayUpdate();
				}
			}
			// IMPORTANT: If your mod is throwing an exception, you really want to
			// catch that exception and disable the behavior.  Remember, the Update()
			// method gets called every frame.  If you've got an exception throwing,
			// it's going to keep throwing every frame and filling up the end-user's
			// log file. Catch the exception here and disable the behavior so that we
			// stop calling the Update() method every frame.
			catch (Exception e) {
				this.Enabled = false;
				Log.Error(e.ToString());
			}
        }

		// This method is called every frame while we're in the game menus.
		// It checks if the top user interface element is the vanilla mods config
		// screen.  If it is, and if the mod is enabled, it swaps in the custom version.
		public virtual void MenusUpdate()
		{
			// Keep track of the user interface element that's currently on the
			// top of the layer stack.
			bool windowChanged = false;
			Window window = this.TopWindow;
			if (window != this.currentWindow) {
				this.currentWindow = window;
				windowChanged = true;
			}

			// If the layer has changed, check the class name to see if it's the vanilla
			// mods config screen.
			if (windowChanged && window != null) {
				if ("RimWorld.Page_ModsConfig".Equals(window.GetType().FullName)) {
					// Check if the mod is enabled.
					if (ModEnabled) {
						ResetTextures();
						ReplaceWindow(window, new Page_ModsConfig());
						Log.Message("Swapped in EdB Mod Order Page");
					}
				}

				// This check will happen every time you go from one menu screen to another.
				// That means it will happen when you close the Mods config screen.  This is a
				// great time to check to see if the mod has been disabled.  If it has been
				// disabled, we unload it.
				if (!ModEnabled) {
					UnloadMod();
				}
			}
		}

		// Instead of using WindowStack.TopWindow(), we define our own logic to get the
		// top layer.  We do this because we want to skip the console log if it's on
		// top.  If we don't do this, all of our logic around swapping in new interface
		// elements fails when the console is open.
		public Window TopWindow
		{
			get {
				// Iterate the layers.
				foreach (Window window in Find.WindowStack.Windows) {
					// A string comparison of the class name here is a little more expensive than
					// we'd like, but we do this instead of comparing the Type to avoid any potential
					// problems with assembly versions.
					if (window.GetType().FullName != "Verse.EditWindow_Log") {
						return window;
					}
				}
				return null;
			}
		}

		// Called to remove the mod and all of the supporting objects that were created along with it.
		protected void UnloadMod()
		{
			// Find the ModInitializer tab class that was created.  RimWorld stores all of its tabs in
			// a Dictionary so that you can look up a tab by its class.  The dictionary field is private
			// so we need to use reflection to get it.  Once we get the dictionary, we remove the tab.
			FieldInfo field = typeof(ITabManager).GetField("sharedInstances", BindingFlags.Static | BindingFlags.NonPublic);
			Dictionary<Type, ITab> sharedInstances = (Dictionary<Type, ITab>)field.GetValue(null);
			sharedInstances.Remove(typeof(ModInitializer));

			// Find the GameObject that we created and destroy it. This will also destroy any
			// components on the game object, including this class' instance.
			GameObject gameObject = GameObject.Find(GameObjectName);
			GameObject.Destroy(gameObject);

			Log.Message("Unloaded the " + ModName + " mod");
		}

		// This mod doesn't do anything during gameplay, but I left this method here
		// as an example.  See the Update() method.
		public virtual void GameplayUpdate()
		{

		}

		// Check to see whether or not the mod is enabled.
		public bool ModEnabled
		{
			get {
				// Find the mod by its name.
				InstalledMod mod = InstalledModLister.AllInstalledMods.First((InstalledMod m) => {
					return m.Name.Equals(ModName);
				});
				// Make sure the mod is installed before returning whether or not it's active.
				if (mod == null) {
					return false;
				}
				return mod.Active;
			}
		}

		// When a mod is deactivated and then later activated, it can cause problems with
		// the mod's textures.  This method gets called by the Start() method to always
		// reload textures when the mod is enabled.  This can avoid missing textures in
		// the mod or the big red X's you sometimes see in the gameplay interface buttons.
		public void ResetTextures()
		{
			Page_ModsConfig.ResetTextures();
		}

		public void ReplaceWindow(Window currentWindow, Window replacement)
		{
			// EdB: We can't call WindowStack.TryRemove() here because that will run the PostClose() logic
			// in the vanilla Page_ModsConfig page, which will reload all of the mod.  Instead we call our
			// own method that mimics TryRemove() without calling the PostClose() method.
			RemoveWindowWithoutClosingIt(currentWindow);
			Find.WindowStack.Add(new Page_ModsConfig());
		}

		// Removes a window from the window stack without calling its Pre/PostClose() methods.  This allows
		// us to replace a window without triggering any logic in the existing window.
		public static void RemoveWindowWithoutClosingIt(Window window)
		{
			FieldInfo windowsField = typeof(WindowStack).GetField("windows", BindingFlags.Instance | BindingFlags.NonPublic);
			FieldInfo focusedWindowField = typeof(WindowStack).GetField("focusedWindow", BindingFlags.Instance | BindingFlags.NonPublic);
			FieldInfo updateInternalWindowsOrderLaterField = typeof(WindowStack).GetField("updateInternalWindowsOrderLater", BindingFlags.Instance | BindingFlags.NonPublic);
			List<Window> windows = (List<Window>) windowsField.GetValue(Find.WindowStack);
			if (!windows.Remove(window)) {
				return;
			}

			// EdB: This is a copy of logic at the end of WindowStack.TryRemove(), using reflection where needed.
			Window focusedWindow = (Window) focusedWindowField.GetValue(Find.WindowStack);
			if (focusedWindow == window) {
				if (windows.Count > 0) {
					focusedWindowField.SetValue(Find.WindowStack, windows[windows.Count - 1]);
				}
				else {
					focusedWindowField.SetValue(Find.WindowStack, null);
				}
				updateInternalWindowsOrderLaterField.SetValue(Find.WindowStack, true);
			}
		}
    }

}
