//FeatureScript 626;
//import(path : "onshape/std/geometry.fs", version : "626.0");

FeatureScript 1271;
import(path : "onshape/std/geometry.fs", version : "1271.0");

import(path : "05ac46458fd6234d1ad55b80", version : "a3d0469ed33d0bb491d9d921");
// herein, we experiment with approximating something like a class, with private members, and functions as members, which have a handle to the calling instance when they run.

type trunion typecheck isTrunion;

predicate isTrunion(value)
{
    
}

function new_trunion() returns trunion
{
    var this is box = new box({});
    var privates is box = new box({});
    
    this[].foo = 33;
    this[].bar = 44;
    privates[].multiplier = 1.4;
    
    this[].getTotal = 
        function()
        {
            return this[].foo + this[].bar;   
        };
        
    this[].incrementTheMultiplier =
        function()
        {
            privates[].multiplier += 1;
        };
    
    this[].getMultiplier =
        function()
        {
            return privates[].multiplier;
        };

    return this as trunion;
   
}


annotation { "Feature Type Name" : "trunion_feature" }
export const trunion_feature = defineFeature(function(context is Context, id is Id, definition is map)
    precondition
    {

    }
    {
       
        
        var myTrunion = new_trunion();
        println("myTrunion[].getTotal(): " ~ myTrunion[].getTotal());
        println("myTrunion[].getMultiplier(): " ~ myTrunion[].getMultiplier());
        myTrunion[].incrementTheMultiplier();
        println("myTrunion[].getMultiplier(): " ~ myTrunion[].getMultiplier());




    });
