//FeatureScript 626;
//import(path : "onshape/std/geometry.fs", version : "626.0");
FeatureScript 1271;
import(path : "onshape/std/geometry.fs", version : "1271.0");
import(path : "4ad564d930bbc1266851ae1a", version : "d8cb0b9b8ababc96e210c7cc");


    

export function myOnFeatureChange(context is Context, id is Id, oldDefinition is map, definition is map, isCreating is boolean, specifiedParameters is map, hiddenBodies is Query) returns map
{
    try{ 
        definition.changeCount += 1; //for debugging, to keep track of how many times this function runs.
    }catch(e) {}

    var newlySelectedNameOfCannedDesign = undefined; //look for the first cannedDesignChoices selected flag that has transitioned from false to true.
    for(var i=0; i<size(definition.cannedDesignChoices); i +=1 )
    {
        if( newlySelectedNameOfCannedDesign == undefined && !oldDefinition.cannedDesignChoices[i].selected && definition.cannedDesignChoices[i].selected)
        {
            newlySelectedNameOfCannedDesign = definition.cannedDesignChoices[i].name;
        }
    }
    
    
    //add and remove items from cannedDesignChoices to ensure that there is exactly one entry in cannedDesignChoices for each canned parameter set
    //construct keyedCannedDesignChoices for convenience below.
    var keyedCannedDesignChoices = {};
    for(var cannedDesignChoice in definition.cannedDesignChoices)
    {
        keyedCannedDesignChoices[cannedDesignChoice.name] = cannedDesignChoice;  
    }
    
    definition.cannedDesignChoices = 
        mapArray(
            keys(getCannedDesigns()),
            function(x)
            {
                var element = keyedCannedDesignChoices[x];
                if(element == undefined)
                {
                    element = 
                        {
                            "name": x,
                            "selected" : false,
                            "displayName" : x
                        };  
                }
                return element;
            }
        );
    if( newlySelectedNameOfCannedDesign != undefined )
    {
        definition.nameOfSelectedCannedDesign = newlySelectedNameOfCannedDesign;      
    }
    for(var i=0; i<size(definition.cannedDesignChoices); i +=1 )
    {
        if(definition.cannedDesignChoices[i].name == definition.nameOfSelectedCannedDesign)
        {
           definition.cannedDesignChoices[i].selected = true; 
           // we don't have to set selected tpo true because selected must necessarily by true for us to get here, but it does not hurt.
           definition.cannedDesignChoices[i].displayName = "(*)  " ~ definition.cannedDesignChoices[i].name; 
        } else {
            definition.cannedDesignChoices[i].selected = false;
            definition.cannedDesignChoices[i].displayName = "( )  " ~ definition.cannedDesignChoices[i].name;
        }
    }
    
    
    

    
    
    
    if(definition.refreshCannedDesignsRequested)
    {
        definition.refreshCannedDesignsRequested = false;
        //do stuff here as desired.
    }
    return definition;
}


//=====================================================
annotation { 
    "Feature Type Name" : "bitHolder", 
    "UIHint": [ "NO_PREVIEW_PROVIDED", "SHOW_EXPRESSION" ], 
    "Editing Logic Function" : "myOnFeatureChange" ,
    "Feature Name Template" : "#nameOfSelectedCannedDesign"
    }
export const bitHolder_feature = defineFeature( function(context is Context, id is Id, definition is map)
    precondition
    {
        
        annotation {"Default": "", "UIHint": ["READ_ONLY"]}
        definition.nameOfSelectedCannedDesign is string;
        

        
        
        annotation { "Name" : "canned designs (select one)",  "Item name" : "canned design", "Item label template" : "#displayName", "UIHint" : ["COLLAPSE_ARRAY_ITEMS",  "READ_ONLY"]}
        definition.cannedDesignChoices is array;
        for (var item in definition.cannedDesignChoices)
        {            
            annotation {"UIHint" : ["ALWAYS_HIDDEN"]}
            item.name is string;
            
            annotation {"UIHint" : ["ALWAYS_HIDDEN"]}
            item.displayName is string;
            
            annotation { "Name" : "click here to select", "UIHint" : ["DISPLAY_SHORT"] }
            item.selected is boolean;
            
        }
        
        annotation { "Name" : "refreshCannedDesigns" }
        definition.refreshCannedDesignsRequested is boolean;   
        
        annotation { "Default":0, "UIHint": ["READ_ONLY", "ALWAYS_HIDDEN"] }
        isInteger(definition.changeCount, POSITIVE_COUNT_BOUNDS);
        
    }
    {  
        return getCannedDesigns()[definition.nameOfSelectedCannedDesign]["featureFunction"](context, id, definition);
    }
);
