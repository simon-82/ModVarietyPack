---
layout: post
comments: false
title: DataTable Growing / Beds
description: 
tags: 
categories: [Information]

---

In an attempt to bring over the content stored on the old wiki, I'll post this here <i class="fa fa-smile-o"></i>    


This is a listing of all plants and their exact values. Better soil allows better plants to grow.    
For example, if you would plant squash on cultivated soil, it wouldn't grow at all because the soil is not fertile enough. 

{% highlight xml %}
Plant scaling: 

Base     (GrowTime 6.2, Yield 8, Nutrition .05)    Potato
Tier I   (GrowTime 6.2, Yield 10, Nutrition .06)   Eggplant
Tier II  (GrowTime 6.0, Yield 10, Nutrition .07)   Mushroom
Tier III (GrowTime 5.0, Yield 11, Nutrition .08)   Carrots (ok raw)

Base     (GrowTime 4.9, Yield 6, Nutrition .05)    Rice
Tier I   (GrowTime 4.4, Yield 6, Nutrition .06)    Squash
Tier II  (GrowTime 4.0, Yield 6, Nutrition .07)    Onion
Tier III (GrowTime 3.2, Yield 6, Nutrition .08)    Lettuce (ok raw)

Base     (GrowTime 13.7, Yield 22, Nutrition .05)  Corn
Tier I   (GrowTime 12.5, Yield 24, Nutrition .06)  Pumpkin
Tier II  (GrowTime 12.4, Yield 24, Nutrition .07)  Bean
Tier III (GrowTime 11.5, Yield 28, Nutrition .08)  Tomato (ok raw)


Fruit Scaling: 
* Agave can't be grown
* all ok eaten raw

Base     (GrowTime 5.1, Yield 7, Nutrition .05)    Strawberry
Tier I   (GrowTime 4.9, Yield 8, Nutrition .06)    Blueberry
Tier II  (GrowTime 4.5, Yield 8, Nutrition .07)    Cloudberry
         (GrowTime 4.5, Yield 8, Nutrition .125)   Pineapple
Tier III (GrowTime 4.4, Yield 10, Nutrition .08)   Gooseberry, Watermelon

Tier IV  FruitTrees (Not destroyed when harvested)
         (GrowTime 18.0, Yield 50, Nutrition .08)  Apple
         (GrowTime 17.8, Yield 50, Nutrition .08)  Orange
         (GrowTime 17.3, Yield 50, Nutrition .08)  Banana
         (GrowTime 17.0, Yield 50, Nutrition .08)  Fig
         (GrowTime 16.2, Yield 50, Nutrition .08)  Peach
         (GrowTime 16.0, Yield 50, Nutrition .08)  Date


Growing (Plant tier per soil):

Base     cultivated (fert. 1.0, FactorGrowthRate 1.0)
Tier I   fertilized (fert. 1.1, FactorGrowthRate 0.5)
Tier II  watered    (fert. 1.2, FactorGrowthRate 0.3)
Tier III enriched   (fert. 1.4, FactorGrowthRate 0.0)
Tier IV  enriched   (fert. 1.4, FactorGrowthRate 0.0)

* All Ressource Plants = Base
{% endhighlight %}  
   
   
Next up are the values for all beds in the modpack. Notice the hospital bed has the highest value for gaining immunity from sickness (or infections) but is not very comfortable. 

{% highlight xml %}
Sleeping Spot (RestEffectiveness 0.5, ImmunityGainSpeedFactor 1.0)
Hide Bed      (RestEffectiveness 0.75, ImmunityGainSpeedFactor 1.0)
Cloth Bed     (RestEffectiveness 1.0, ImmunityGainSpeedFactor 1.05)
Royal Bed     (RestEffectiveness 1.25, ImmunityGainSpeedFactor 1.05)
Comfy Bed     (RestEffectiveness 1.25, ImmunityGainSpeedFactor 1.075)
Pod Bed       (RestEffectiveness 1.5, ImmunityGainSpeedFactor 1.1)

Hospital Bed  (Rest 1.0, ImmunityGain 1.144)
{% endhighlight %}