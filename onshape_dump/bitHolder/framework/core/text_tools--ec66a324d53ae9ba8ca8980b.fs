//FeatureScript 626;
//import(path : "onshape/std/geometry.fs", version : "626.0");

FeatureScript 1271;
import(path : "onshape/std/geometry.fs", version : "1271.0");

import(path : "e74f2213a201f3810593935d", version : "e7bdd71357900dc72524add5");
import(path : "20be3e9e3b832f6babff03e7", version : "fe25cd47b70ccda1fd60e890");
import(path : "05ac46458fd6234d1ad55b80", version : "a3d0469ed33d0bb491d9d921");


        // var fontNames = 
        //     [
        //         "OpenSans-Regular.ttf",         // Sans-serif font. Default if no match is found. 
        //         "AllertaStencil-Regular.ttf",   // Stencil font. No bold/italic options. 
        //         "Arimo-Regular.ttf",            // Sans-serif font. 
        //         "DroidSansMono-Regular.ttf",    // Monospaced sans-serif font. No bold/italic options. 
        //         "NotoSans-Regular.ttf",         // Sans-serif font. 
        //         "NotoSansCJKjp-Regular.otf",    // Japanese font. No italic options. 
        //         "NotoSansCJKkr-Regular.otf",    //  Korean font. No italic options. 
        //         "NotoSansCJKsc-Regular.otf",    //  Chinese (simplified) font. No italic options. 
        //         "NotoSansCJKtc-Regular.otf",    //  Chinese (traditional) font. No italic options. 
        //         "NotoSans-Regular.ttf",         //  Serif font. 
        //         "RobotoSlab-Regular.ttf",        //  Sans-serif font. No italic options. 
        //         "Tinos-Regular.ttf"              // Serif font. Metrically compatible with Times New Roman.
        //     ];


type sketchTextData typecheck isSketchTextData;

predicate isSketchTextData(x)
{
    x is map;   
}

function getSketchTextData(context is Context, idOfSketch is Id, sketchEntityIdOfTheSketchText is string) returns sketchTextData
{
    // I discovered the various sketchEntityIdSuffixes for all the elements of 
    // a sketchText item by setting a contraint (by means of the UI) on each of the lines and 
    // points in the text (not the edges of the characters, which are not directly manipulable in the UI, 
    // but the construction lines that belong to the text and which are manipulable)
    // , and then looking at the resulting code of the 
    // Feature Studio, particular the "localFirst" argument to the skConstraint() 
    // calls that corresponded to the constraints that I had added in the ui.      
    
    
    var sketchTextSuffixes = 
        [                
            //"baseline" is the horizontal rule below the line of text
            "sketch_text.baseline",           // entity type is EntityType.EDGE
            "sketch_text.baseline.start",     // entity type is EntityType.VERTEX
            "sketch_text.baseline.end",       // entity type is EntityType.VERTEX
            
            //"ascent" horizontal rule above the line of text
            "sketch_text.ascent",             // entity type is EntityType.EDGE
            "sketch_text.ascent.start",       // entity type is EntityType.VERTEX
            "sketch_text.ascent.end",         // entity type is EntityType.VERTEX
            
            //"left" is the vertical rule to the left of the line of text
            "sketch_text.left",               // entity type is EntityType.EDGE
            "sketch_text.left.start",         // entity type is EntityType.VERTEX
            "sketch_text.left.end",           // entity type is EntityType.VERTEX
            
            //"right" is the vertical rule to the right of the line of text
            "sketch_text.right",              // entity type is EntityType.EDGE
            "sketch_text.right.start",        // entity type is EntityType.VERTEX
            "sketch_text.right.end",          // entity type is EntityType.VERTEX
            
            //"center" is the vertical rule in the center of th eline of text
            "sketch_text.center",             // entity type is EntityType.EDGE
            "sketch_text.center.start",       // entity type is EntityType.VERTEX
            "sketch_text.center.end",         // entity type is EntityType.VERTEX
        ];
            
                        
    
    
    // println("numberOfEntitiesReturnedByQuery(context,sketchEntityQuery(idOfSketch, undefined,\"\")): " ~ numberOfEntitiesReturnedByQuery(context,sketchEntityQuery(idOfSketch, undefined,"")));
    // println("numberOfEntitiesReturnedByQuery(context,sketchEntityQuery(idOfSketch, undefined,\"*\")): " ~ numberOfEntitiesReturnedByQuery(context,sketchEntityQuery(idOfSketch, undefined,"*")));
    
    
    // //the following sizes are the same.  (i.e. All of the entities returned by a call to qCreatedBy(x), where x is the Id of a sketch feature, are considered to be SketchObjects by the qSketchFilter function.
    // println("numberOfEntitiesReturnedByQuery(context, qCreatedBy(idOfSketch)): " ~ numberOfEntitiesReturnedByQuery(context, qCreatedBy(idOfSketch)));
    // println("numberOfEntitiesReturnedByQuery(context, qSketchFilter(qCreatedBy(idOfSketch), SketchObject.YES)): " ~ numberOfEntitiesReturnedByQuery(context, qSketchFilter(qCreatedBy(idOfSketch), SketchObject.YES)));
    // println("numberOfEntitiesReturnedByQuery(context, qCreatedBy(idOfSketch + sketchEntityIdOfTheSketchText)): " ~ numberOfEntitiesReturnedByQuery(context, qCreatedBy(idOfSketch + sketchEntityIdOfTheSketchText)));
    
    
    
    // generate a map, called data, whose keys are the suffix of sketchEntityId's of the entities owned by the sketchText.
    // when I say 'suffix', I mean the part of the sketchEntityId that follows the "." that follows the sketchEntityId of the sketchText.
    // Each of the map's values is either a 3d vector or a Line (i.e. the onshape 'Line' type, which consists of a root point and
    // a direction vector (but not endpoints)),
    // according to whether the entity is an EDGE or a VERTEX, respectively.
    var data = {};
    for(var sketchTextSuffix in sketchTextSuffixes)
    {
        var key = sketchTextSuffix;
        var q = sketchEntityQuery(
            idOfSketch, //id of the sketch feature,
            undefined, //item.entityType, // the entity type that you want to retrieve (or undefined to retrieve all types)
            sketchEntityIdOfTheSketchText ~ "." ~ sketchTextSuffix // the sketchEntityId of the sketch entity in question (this is the string that we pass in to the various sk... functions) OR, and here's the easter egg, a dot delimited string that starts with that id and then can have sub-parts. ( a sketch line for instance, has a ".start" and a ".end" property/id, which are the ids of the vertices.)
            );
        //println("idOfSketch: " ~ idOfSketch);		// idOfSketch
        //println("sketchEntityIdOfTheSketchText: " ~ sketchEntityIdOfTheSketchText);		// sketchEntityIdOfTheSketchText
        
        q = qSketchFilter(q, SketchObject.YES);
        
        q = qSubtraction(q, qEverything(EntityType.BODY)); //ignore bodies (we only want the consituent edges, vertices (and surfaces, although there will not be any in our current application .)
        
  
       // println("sketchTextSuffix: " ~ sketchTextSuffix); print(reportOnEntities(context,q,1));
        
        if (!queryReturnsExactlyOneEntity(context,q))
        {
            println("numberOfEntitiesReturnedByQuery(context,q): " ~ numberOfEntitiesReturnedByQuery(context,q));
        }
        else
        {
            if(isVertex(context, q)) 
            {
                data[key] = evVertexPoint(context,    {"vertex" : q}   );
            } else if (isEdge(context, q))
            {
                // data[key] = evLine(context,    {"edge" : q}  );
                data[key] = evCurveDefinition(context,    {"edge" : q}  );
            } else
            {
                continue;
            }
        } 
    }
    
    data["basePoint"] = data["sketch_text.baseline.start"];
    data["direction"] = data["sketch_text.baseline"].direction; //this is the forward reading direction of the text.  It is "rightward" in the sense tht we use 'right' and ;eft' when we say that english text is read left-to-right.
    data["paperPlane"] = 
        plane(
            /*origin*/ data["basePoint"],
            /*normal*/ evOwnerSketchPlane(context, {"entity":qCreatedBy(idOfSketch)}).normal,
            /*x*/ data["direction"]
        );//this is an onshape 'Plane' that contains the sketchText sheet bodies, oriented so that the 'x' axis of the plane is parallel to data["direction"], positioned so that the origin of the plane is coincident with data["basePoint"];
    data["nominalWidth"] = norm(data["sketch_text.baseline.end"] - data["sketch_text.baseline.start"]); //this is the nominal width of the line of text, as read off of the length of the sketchText's entities baseLine construction line segment. 
    data["nominalHeight"] = 
        norm(
            project(
                yAxis(data["paperPlane"]),
                data["sketch_text.baseline.start"] - data["sketch_text.ascent.start"]
            )  //I probably don't strictly need to do this project rigamarrol, but I am trying to be tolerant of future changes that OnShape might make to the definition of the ascent line (what if they reverse the ascent line, so that ascent.start is no longer vertically aligned with baseline.start) -- the projection will work with more potential future changes than simply assuming that baseline.start is vertically aligned with ascent.start.
        ); //this is the nominal height of the line of the text, as read off of the vertical distance between the baseLine and the ascent (both of which are contstruction line segments that are part of the sketchText entity
    
    // use onshape ev... evaluation functions to find the boundingBox of the text (which is, in general, not quite the same as the nominal width and height), due to, for instance, the descender on a lower case y sticking down below the baseline.  The nominal values are merely reference lines that for the vector font defintition, which the font is free to print beyond, or remain tightly confined within.
    
    
    return data as sketchTextData;           
}

// a text row represents a row of text on a textLayout (think of a textLayout as being one piece of paper), which contains a member function to generate the sheet bodies.
export type textRow typecheck isTextRow;

export predicate isTextRow(x)
{
    x is box;
    x[] is map;
}

export function new_textRow() returns textRow
{
    var this is box = new box({});
    var private is box =  new box({}); //this stores private members
    

    this[].height = 1 * inch; //isLength ///when we go to create sheet bodies on the galley, we will look at this value and scale the size of the bodies accordingly.  We will scale the size of the bodies so that the nominal height of the text (as would be measured from the automatically-gnerated cotruction line segmens that an OnShape sketchText sketch entity generates.) will become this[].fontHeight.
    this[].basePoint = vector(0,0,0) * meter; //is3dLengthVector  //this is a vector, in galleySpace (galleySpace is 3 dimensional, although only the x and y dimensions are significant for the final results. (When we are building bodies for a layout, we first create all the bodies "in galleySpace" and then transform them all by the galleyToWorld transform.  
    
    // The following comment was written before I decided to use the name "galley".  Prior to 'galley', I was unsing the word "textLayout" to refer to galley.
    //  (The 'paper' here is not to be confused with the 'paper' on which a 2d drawing of this 3d model might be drawn).  (I really need to come up with a better word than 'paper' for the current project, because "paper" is already used in the context of making 2d drawings.  Perhaps poster is a better word.  Broadside, banner, billboard, marquee, signboard, foil. lamina, saran wrap, plastic wrap, cling wrap, film, membrane, stratum, veneer, mat, varnish, skin, graphicSkin, veil, screen, facade, parchment, velum, fabric, leaf, inkMembrane, inkSpace, inkSheet, inkScape, placard, plaque, plate, proofPlate, blackboard, whiteboard, readerboard, engraving, galley (in the context of a printing press) - a galley is the tray into which a person would lay type (e.g. lead (Pb) type - think 'printing press') into and tighten the type into place.  This is exactly the notion I am after, and "galley" does not have any alternate uses in the context of 3d geomtric modeling.  The galley isn't really an solid object within the model - it is a tool that can be brought to bear on the model to make engraves in the solid of the model (or embosses - yes, that stretches the analogy a bit, but the concept is close enough), and then, when you are finished with the galley, you put it back in the drawer from whence it came.  In other words, the galley itself is not part of the final finsihed model, but the marks that the galley makes are part of the finished model.
    
    
    
    private[].text = ""; //this value can (and probably will be) changed later by the calling code (via this[].set()).
    private[].fontName = "Tinos-Italic.ttf"; //this value can (and probably will be) changed later by the calling code (via this[].set()).
    private[].owningGalley = undefined; //this will be set to a galley object when the time comes to add this textRow to a particular owning textLayout. 
    
    private[].scaleFreeShapeParameters = ["scaleFreeHeight", "scaleFreeWidth", "scaleFreeDepth"]; //'depth' here is in the TeX sense of the word (depth is the amount by which the characeters protrude below the baseline)
    // the members of scaleFreeShape are nuumbers with length units, but any one of these values is not significnt in itself -- what matters is the ratio between 
    // and among pairs of 'scaleFree' dimensions.  These ratios describe the shape of the textRow irrespective of scale.
    

    private[].shapeChangers = ["text", "fontName"];
    // these are the names of properties of private[], which will, when changed, affect the scaleFreeShapeParameters.  We will allow the user to set these properties (via a setter function), 
    // but the setter function will turn on the shapeIsDirty flag so that we will know that we need to recompute the shape if we want good shape data.
    
    
    private[].shapeIsDirty = true; 
    //this is a flag that we will set to true whenever any of the parameters designated in shapeChangers is set.
    
    
    private[].getablePrivates = ["text", "fontName", "owningGalley", "scaleFreeHeight", "scaleFreeWidth", "scaleFreeDepth"]; //"width" is computed on demand from the scaleFreeShape and the fontHeight, which is the one parameter that 
    //whenever the calling code uses this[].set() to set a private member, if the name of the private member is in private[].shapeChangers, we recompute the scaleFreeShapeParameters
    
    
    private[].computeShape = 
        function()
        {
            private[].buildScaleFreeSheetBodies(newContextWithDefaults(), makeId("dummyDummyDummy"));
            //  As a side effect, private[].buildScaleFreeSheetBodies() computes and sets the scale free shape parameters, so we merely have to let it run in a temporary dummy context.  We don't care about saving the results of the build in the temporary context, we are just getting the data we need and then letting the temporary context be destroyed by garbage collection. 
        };
    
    
    //this function constructs (And returns a query for) one or more sheet bodies, all lying on the xy plane.
    // as a side effect, this function computes scaleFreeShapeParameters
    
    //we are assuming that the sheet bodies will be positioned so that the basePoint of the row of text is at the origin, the 
    // normal of the plane that the text is on points in the positive z direction, and the reading direction of 
    //  the text points in the positive x direction.
    private[].buildScaleFreeSheetBodies = 
        function(context is Context, id is Id) //returns Query that resolves to a set of sheet bodies (or possibly a single sheet body with multiple disjoint faces, if such a thing is allowed), all lying on the xy plane.
        {
            var idOfWorkingSketch = id + "workingSketch";
            var workingSketch is Sketch = newSketchOnPlane(context, idOfWorkingSketch, {    "sketchPlane":XY_PLANE     });
            var sketchEntityIdOfSketchText = "neilsText";
            var textIdMap;
            try{  
               textIdMap = 
                    skText(workingSketch, sketchEntityIdOfSketchText,
                        {
                            "text": this[].get_text(),
                            "fontName": this[].get_fontName(),
                        }
                    );
                //println("textIdMap: " ~ textIdMap);
            } catch(error) {
                println("skText threw an excpetion.");   
            }
            try{skSolve(workingSketch);}
            //debug(context, qConstructionFilter(qCreatedBy(idOfWorkingSketch, EntityType.EDGE), ConstructionObject.YES));
            var mySketchTextData;
            try{mySketchTextData = getSketchTextData(context, idOfWorkingSketch, sketchEntityIdOfSketchText);}
            try{private[].scaleFreeHeight = mySketchTextData.nominalHeight;}
            try{private[].scaleFreeWidth = mySketchTextData.nominalWidth;}
            
            //println("mySketchTextData: " ~ mapToString(mySketchTextData));
            
            var idOfBodyCopy = id + "bodyCopy";
            
            //opPattern creates copies of the specified bodies.
            // we want to create a copy of the sheetBodies in the sketch, so that we have a copy, which is independent of the original sketch, so that we can then delete the sketch and be left with just the faces.
            
            //qSketchRegion returns all faces of all sheetBodies in the sketch if the second argument (filterInnerLoops) is false, or else the all the 'outer' faces of all sheetBodies in the sketch if filterInnerLoops is true.
            var inkSheets is Query = qBodyType( qEntityFilter( qOwnerBody(qSketchRegion(idOfWorkingSketch,false)), EntityType.BODY), BodyType.SHEET) ;  //I realize that some of these filters are probably redundant - I just want to be darn sure that I am picking out exactly what I want (namely all sheetBodies in the sketch) and nothing else.
            
            //delete all the even-rank faces (this concept of ranking of faces of a planar sheetBody is my own terminology -- not from the OnShape documentation.)
            deleteEvenRankFaces(context, id + "deleteEvenRankFaces", inkSheets);
            
            //To DO: use onshape ev... evaluation functions to find the actual bounding box of the glyphs.
            
            
            try silent{ 
                opPattern(context, idOfBodyCopy,
                    {
                        entities: inkSheets, 
                        transforms: [ identityTransform()],
                       //transforms: [ transform(vector(0,-3,0) * meter)], //for debugging.
                        instanceNames: [uniqueIdString(context)]
                    }
                );
            } 

            var scaleFreeSheetBodies = qBodyType(qEntityFilter(qCreatedBy(idOfBodyCopy), EntityType.BODY), BodyType.SHEET);   
            
            
            //print(reportOnEntities(context, inkSheets,0,0));
            //debug(context, inkSheets);
            //debug(context, qCreatedBy(idOfBodyCopy));
            
            //debug(context, qCreatedBy(idOfWorkingSketch));
            
            //get rid of all the entities in the sketch, which we do not need now that we have extracted the sheetBodies that we care about.
            try silent{opDeleteBodies(
                context,
                uniqueId(context, id),
                {entities:qCreatedBy(idOfWorkingSketch)}
            );  } 
            
            
            private[].shapeIsDirty = false;
            //debug(context, qCreatedBy(idOfWorkingSketch));
            return scaleFreeSheetBodies;
        };
   
    
    
    // builds the sheetBodies in galley space, returns the resulting sheetBodies
    this[].buildSheetBodiesInGalleySpace = 
        function(context is Context, id is Id)
        {
            var scaleFreeSheetBodies = private[].buildScaleFreeSheetBodies(context, id + "buildScaleFreeSheetBodies");
            //scale and translate and scaleFreeSheetBodies according to this[].get("height") and this[].get("basePoint")
            
            var idOfTransformOperation = "scaleFreeTextRowToGalley";

            try{
                opTransform(context, id + idOfTransformOperation,
                    {
                        "bodies": scaleFreeSheetBodies,
                        "transform": transform(this[].get_basePoint()) * scaleUniformly(this[].get_height()/this[].get_scaleFreeHeight())    
                    }
                );
            }
            
            
            return scaleFreeSheetBodies;  // I am assuming the the query for the bodies still refers the (now transformed bodies).
        };
    
    
    // ==== GETTERS AND SETTERS
    // this[].set = 
    //     function(propertyName is string, newValue) //text is expected to be a string with no newlines.
    //     {
    //         if (    isIn(propertyName, keys(this[]))    )
    //         {
    //             this[][propertyName] = newValue; //this gives access to members that are already public, for convenience, and uniform notation.
    //         } else if(propertyName == "text")
    //         {
    //             private[][propertyName] = newValue;   
    //         } else if (propertyName == "fontName")
    //         {
    //             private[][propertyName] = newValue;  
    //         } 
            
    //         if( isIn(propertyName, private[].shapeChangers)) 
    //         {
    //             private[].computeShape();
    //         }
    //         return newValue;   
    //     }; 
    
    //allows the calling code to have readOnly access to the private members.  Perhaps it would be better to have a list of names of readOnly members, so that the private calues could remain truly private.
    // this[].get = 
    //     function(propertyName is string)
    //     {
    //         if (   isIn(propertyName, keys(this[]))   )
    //         {
    //             return this[][propertyName]; //this gives access to members that are already public, for convenience.   
    //         } else if (   isIn(propertyName, private[].getablePrivates)   )
    //         {
    //             return private[][propertyName];
    //         } else if (propertyName == "width") 
    //         {
    //             return this[].get("height") * private[].scaleFreeWidth/private[].scaleFreeHeight;
    //         }
    //     };
    
    
    // create a getters and setters
    addDefaultGettersAndSetters(this);
    
    for(var propertyName in private[].getablePrivates)
    {
        if(isIn(propertyName, private[].scaleFreeShapeParameters))
        {
            this[]["get_" ~ propertyName] = 
            function(){
                //recompute the shape if the shape data is out of date ( private[].computeShape() will clear the shapeIsDirty flag.)
                if(private[].shapeIsDirty){private[].computeShape();}
                return private[][propertyName];
            };   
        } else
        {
             this[]["get_" ~ propertyName] = function(){return private[][propertyName];};   
        }
    }
    
    this[]["get_" ~ "width"] = function(){
        return this[].get_height() * this[].get_scaleFreeWidth()/this[].get_scaleFreeHeight();
        };
    
    
    for(var propertyName in private[].shapeChangers)
    {
        this[]["set_" ~ propertyName] = 
            function(newValue)
            {
                private[][propertyName] = newValue;
                private[].shapeIsDirty = true;
                return newValue;
            };
    }
    this[]["set_" ~ "owningGalley"] = function(newValue){private[]["owningGalley"] = newValue; return newValue;};
        
    return this as textRow;
}


// filledRectangle
// a text row represents a row of text on a textLayout (think of a textLayout as being one piece of paper), which contains a member function to generate the sheet bodies.
export type filledRectangle typecheck isFilledRectangle;

export predicate isFilledRectangle(x)
{
    x is box;
    x[] is map;
}

export function new_filledRectangle() returns filledRectangle
{
    var this is box = new box({});
    var private is box =  new box({}); //this stores private members
    
    this[].corner1 = vector(0,0) * meter;
    this[].corner2 = vector(1,1) * meter;
    
    //this function constructs (And returns a query for) one sheet body, all lying on the xy plane.
    this[].buildSheetBodiesInGalleySpace = 
        function(context is Context, id is Id) //returns Query that resolves to a set of sheet bodies (or possibly a single sheet body with multiple disjoint faces, if such a thing is allowed), all lying on the xy plane.
        {
            var idOfWorkingSketch = id + "workingSketch";
            var workingSketch is Sketch = newSketchOnPlane(context, idOfWorkingSketch, {    "sketchPlane":XY_PLANE     });
            var sketchRectangleId = "neilsRectangle";
            try{  
               var result = 
                    skRectangle(workingSketch, sketchRectangleId,
                        {
                            "firstCorner": this[].get_corner1(),
                            "secondCorner": this[].get_corner2(),
                            "construction": false,
                        }
                    );
            } catch(error) {
                println("skRectangle threw an excpetion.");   
            }
            try{skSolve(workingSketch);}
            //debug(context, qConstructionFilter(qCreatedBy(idOfWorkingSketch, EntityType.EDGE), ConstructionObject.YES));

            var idOfBodyCopy = id + "bodyCopy";
            
            //opPattern creates copies of the specified bodies.
            // we want to create a copy of the sheetBodies in the sketch, so that we have a copy, which is independent of the original sketch, so that we can then delete the sketch and be left with just the faces.
            
            //qSketchRegion returns all faces of all sheetBodies in the sketch if the second argument (filterInnerLoops) is false, or else the all the 'outer' faces of all sheetBodies in the sketch if filterInnerLoops is true.
            var inkSheets is Query = qBodyType( qEntityFilter( qOwnerBody(qSketchRegion(idOfWorkingSketch,false)), EntityType.BODY), BodyType.SHEET) ;  //I realize that some of these filters are probably redundant - I just want to be darn sure that I am picking out exactly what I want (namely all sheetBodies in the sketch) and nothing else.
            
            //println("reportOnEntities(context,inkSheets,0): " ~ toString(reportOnEntities(context,inkSheets,0)));
            //debug(context, sheetBodiesInGalleySpace);
            
            
            //delete all the even-rank faces (this concept of ranking of faces of a planar sheetBody is my own terminology -- not from the OnShape documentation.)
            deleteEvenRankFaces(context, id + "deleteEvenRankFaces", inkSheets); //probably not strictly necessary in the case of a simple rectangle.
            
            try silent{ 
                opPattern(context, idOfBodyCopy,
                    {
                        entities: inkSheets, 
                        transforms: [ identityTransform()],
                       //transforms: [ transform(vector(0,-3,0) * meter)], //for debugging.
                        instanceNames: [uniqueIdString(context)]
                    }
                );
            } 

            var scaleFreeSheetBodies = qBodyType(qEntityFilter(qCreatedBy(idOfBodyCopy), EntityType.BODY), BodyType.SHEET);   
            
            
            //print(reportOnEntities(context, inkSheets,0,0));
            //debug(context, inkSheets);
            //debug(context, qCreatedBy(idOfBodyCopy));
            
            //debug(context, qCreatedBy(idOfWorkingSketch));
            
            //get rid of all the entities in the sketch, which we do not need now that we have extracted the sheetBodies that we care about.
            try silent{opDeleteBodies(
                context,
                uniqueId(context, id),
                {entities:qCreatedBy(idOfWorkingSketch)}
            );  } 
            //println("reportOnEntities(context,scaleFreeSheetBodies,0): " ~ toString(reportOnEntities(context,scaleFreeSheetBodies,0)));
            return scaleFreeSheetBodies;
        };
   
    // create a getters and setters
    addDefaultGettersAndSetters(this);
    
    return this as filledRectangle;
}

export function new_filledRectangle(initialSettings) returns filledRectangle
{
    var this = new_filledRectangle();
    this[].set(initialSettings);
    return this;
}














// "textLayout" is not quite the word for the type of object I am creating.
// this will be an object that contains and manipulates a "page" (in the sense of a typographical page)
// which can be filled with text (and, eventually, perhaps tables, icons, and all manner of black-and-white vector graphics -- I imagine that, in some highly advanced state, this 
// functionality would evolve to be able to ingest TeX or postscript code.)
// and, then, upon demand, rendered as a sheet body (a set of faces (each face being one connected blob of "ink" on the "page")) 
// which can then be extruded (or even wrapped and warped) as desired to generate a meaningful solid.
// Maybe I should invoke one of the technical terms related to those clear plastic sheets with cut-out viyl patches on them, that are used
// to transfer vinyl signs onto the side of a truck or onto a piece of glass, for instance.  That sort of thing is very similar to the 
// sort of object that I am creating -- a flat workspace/template/layout desk/ that can be filled with black-and-white vector graphics,
// and then used as a sheet body in the model.

export type galley typecheck isGalley;
export predicate isGalley(x)
{
    x is box; 
    x[] is map;
}

export enum horizontalAlignment
{
    LEFT,
    CENTER,
    RIGHT
}

export enum verticalAlignment
{
    TOP,
    CENTER,
    BOTTOM
}

export enum galleyAnchor_e
{
    TOP_LEFT,     TOP_CENTER,     TOP_RIGHT,
    CENTER_LEFT,  CENTER,         CENTER_RIGHT,
    BOTTOM_LEFT,  BOTTOM_CENTER,  BOTTOM_RIGHT
}



export function new_galley() returns galley
{
    var this is box = new box({});
    
    this[].width = 8.5 * inch;
    this[].height = 11 * inch;
    this[].clipping = true; //this parameter controls whether to delete parts of the sheet bodies that extend beyond the galley boundary (determined by the width and height parameters above).  In galleySpace (the frame that is fixed to the galley) (the lower left corner of the galley is the origin).
    
    
    this[].fontName = "Tinos-Regular.ttf";
    this[].rowHeight = 10/72 * inch;
    this[].rowSpacing = 1; //this is a unitless ratio that sets the ratio of the vertical interval between successive textRows and the rowHeight.
    
    this[].horizontalAlignment = horizontalAlignment.LEFT; 
    this[].verticalAlignment = verticalAlignment.TOP;
    
    this[].leftMargin    = zeroLength;
    this[].rightMargin   = zeroLength;
    this[].topMargin     = zeroLength;
    this[].bottomMargin  = zeroLength;
    
    this[].worldPlane = XY_PLANE; 
    
    //the anchor specifies which point in galley space will be mapped to the origin of worldPlane.
    // this[].anchor may be any of the following:
    //  a galleyAnchor_e enum value
    //  a 3d length vector (according to is3dLengthVector()), in which case this will be taken as the point in galley space to be mapped to the origin of worldPlane
    //  a unitless vector having at least 2 elements (Accordng to size() >= 2 and isUnitlessVector()), in which case the elements of this[].anchor will taken to be a scale factor to be applied to width and height, respectively (in the spirit of the scaled position that Mathematica uses for many of its graphics functions)
    this[].anchor = galleyAnchor_e.BOTTOM_LEFT; 
    
    //the text boxes will be aligned with the margin (which is a rectangular region that is inset from the edges of the galley according to the margin values above.

    this[].text = "";
    // this[].fontName = "Tinos-Italic.ttf";
    // this[].fontHeight = 12/72 * inch;

        
    this[].buildSheetBodiesInGalleySpace = 
        function(context is Context, id is Id)
            {
                var sanitizedText = "";
                //parse tex-style directives from the text.
                var regExForTexDirective = "(.*?)(?:\\\\(\\w+)(?:\\{(.*?)\\}|))(.*)";
                var texDirectives = [];
                var remainder = this[].get_text();
                var result = match(remainder, regExForTexDirective) ;
                //println("result: " ~ toString(result));		// 
                while(result.hasMatch)
                {
                    sanitizedText ~= result.captures[1];
                    texDirectives =  append(texDirectives, 
                        {
                            "controlWord" : result.captures[2],
                            "argument" : result.captures[3]
                        }
                    );
                    remainder = result.captures[4];
                    result = match(remainder, regExForTexDirective) ;
                    //println("result: " ~ toString(result));		// 
                }
                sanitizedText ~= remainder;
                //println("regExForTexDirective: " ~ toString(regExForTexDirective));		// 
                //println("texDirectives: " ~ toString(texDirectives));		// 
                //println("sanitizedText: " ~ toString(sanitizedText));		// 
                var floodWithInk = false;
                if(
                    isIn(
                        "floodWithInk",
                        mapArray(texDirectives,
                            function(x){return x["controlWord"];}
                        )
                    )
                )
                {
                    //println("floodWithInk directive detected");  
                    floodWithInk= true;
                }
                
                
                
                var sheetBodiesInGalleySpace = qNothing(); 
                //we will use sheetBodiesInGalleySpace as a container, which we willl fill up with sheet bodies.
                //By the time we are done working, sheetBodiesInGalleySpace will contain all the the sheetBodies that we want.
             
                if(floodWithInk)
                {
                    sheetBodiesInGalleySpace = 
                        qUnion([
                            sheetBodiesInGalleySpace,
                            // rectangle from [0,0] to [this[].get_width(), this[].get_height()]  //thisTextRow[].buildSheetBodiesInGalleySpace(context, uniqueId(context, id))
                            new_filledRectangle({"corner1": vector(0,0) * meter, "corner2": vector(this[].get_width(), this[].get_height())})[].buildSheetBodiesInGalleySpace(context, uniqueId(context, id))
                        ]);
                    //println("reportOnEntities(context,sheetBodiesInGalleySpace,0): " ~ toString(reportOnEntities(context,sheetBodiesInGalleySpace,0)));
                } else {
                    // lay down the text boxes
                    { 
                        var linesOfText = explode("\n", sanitizedText);
                        var initX;
                        var initY;
                        var initZ = zeroLength;
                        
                                            
                        //the following tthree lines allow the fontName, rowHeight, and rowSpacing to be either arrays or single values.
                        // if an array, we will cycle through the values in the array as we create one row after another.
                        var fontNameArray = (this[].get_fontName() is array ? this[].get_fontName() : [this[].get_fontName()]);
                        var rowHeightArray = (this[].get_rowHeight() is array ? this[].get_rowHeight() : [this[].get_rowHeight()]);
                        var rowSpacingArray = (this[].get_rowSpacing() is array ? this[].get_rowSpacing() : [this[].get_rowSpacing()]); // the entries in the row spacing array affect how much space will exist between a row and the row above it. (thus, row spacing for the first row has no effect - only for rows after the first row.)
                        
                        //verticalRowInterval is the vertical distance that we will move the insertion point between successive rows.
                        //var verticalRowInterval = this[].get_rowHeight() * this[].get_rowSpacing(); 
                        
                        
                        //heightOfAllText is the distance from the baseline of the bottom row to the ascent of the top row, when all rows are laid out.
                        //var heightOfAllText = verticalRowInterval * size(linesOfText);
                        var heightOfAllText = rowHeightArray[0];
                        for(var i = 1; i<size(linesOfText); i+=1)
                        {
                            //this[].get_rowHeight() + (size(linesOfText)-1)*verticalRowInterval;
                            heightOfAllText += rowSpacingArray[i % size(rowSpacingArray)] * rowHeightArray[i % size(rowHeightArray)];
                        }
                        
                        if(  this[].get_horizontalAlignment() == horizontalAlignment.LEFT )
                        {
                            initX = this[].get_leftMargin();
                        } else if (this[].get_horizontalAlignment() == horizontalAlignment.CENTER)
                        {
                            initX = mean([ this[].get_leftMargin(), this[].get_width() - this[].get_rightMargin() ]);
                        }
                        else // if (this[].get_horizontalAlignment() == horizontalAlignment.RIGHT)
                        {
                            initX = this[].get_width() - this[].get_rightMargin();
                        }
                        
                        
                        
                        if(  this[].get_verticalAlignment() == verticalAlignment.TOP )
                        {
                            initY = this[].height - this[].topMargin - rowHeightArray[0];
                        } else if (this[].get_verticalAlignment() == verticalAlignment.CENTER)
                        {
                            initY = 
                                mean([this[].get_height() - this[].get_topMargin(), this[].get_bottomMargin()]) //this is the y-coordinate of the vertical center
                                + heightOfAllText/2 
                                - this[].get_rowHeight();
                        }
                        else // if(  this[].get_verticalAlignment() == verticalAlignment.BOTTOM )
                        {
                            initY = this[].get_bottomMargin() + heightOfAllText - rowHeightArray[0];
                        }
                        
                        var insertionPoint = vector(initX, initY , initZ);
    
                        
                        for(var i = 0; i<size(linesOfText); i+=1)
                        {
                            var lineOfText = linesOfText[i];
                            var thisTextRow =  new_textRow()  ;
                            
                            thisTextRow[].set_owningGalley(this);
                            thisTextRow[].set_text(lineOfText);
                            thisTextRow[].set_fontName(fontNameArray[i % size(fontNameArray)]);
                            thisTextRow[].set_height(rowHeightArray[i % size(rowHeightArray)]);
                            
                            
                            if(  this[].get_horizontalAlignment() == horizontalAlignment.LEFT )
                            {
                                 thisTextRow[].set_basePoint(insertionPoint);
                            } else if (this[].get_horizontalAlignment() == horizontalAlignment.CENTER)
                            {
                                 thisTextRow[].set_basePoint(insertionPoint - thisTextRow[].get_width()/2 * vector(1, 0, 0));
                            }
                            else // if(  this[].get_horizontalAlignment() == horizontalAlignment.RIGHT )
                            {
                                thisTextRow[].set_basePoint(insertionPoint - thisTextRow[].get_width() * vector(1, 0, 0));
                            }
                            
                            if(i<size(linesOfText)-1) //if we are not on the last row
                            {
                                insertionPoint += -yHat * rowSpacingArray[i+1 % size(rowSpacingArray)] * rowHeightArray[i+1 % size(rowHeightArray)]; //drop the insertion point down to be ready to start the next row.
                            }
                            
                           
                            
                            sheetBodiesInGalleySpace = 
                                qUnion([
                                    sheetBodiesInGalleySpace,
                                    thisTextRow[].buildSheetBodiesInGalleySpace(context, uniqueId(context, id))
                                ]);
                        }
                    }
                }
                //apply clipping, if requested.
                if(this[].get_clipping())
                {
                    
                    var idOfGalleyMask = uniqueId(context, id);
                    var idOfClipping = uniqueId(context, id);                
                    var idOfTextExtrude = uniqueId(context, id);
                    //construct the galleyMask.  This is a region outside of which we will not allow the galley to have any effect.  
                    // (We will do a boolean intersection between galleyMask and the sheet bodies created above.
                   
                    fCuboid(
                        context,
                        idOfGalleyMask,
                        {
                            corner1:vector(0,0,-1) * millimeter,
                            corner2:vector(this[].get_width() , this[].get_height() , 1 * millimeter)
                        }
                    );
                    var galleyMask = qCreatedBy(idOfGalleyMask, EntityType.BODY);
                    //println("reportOnEntities(context,galleyMask,0): " ~ toString(reportOnEntities(context,galleyMask,0)));
                    //debug(context, qOwnedByBody(sheetBodiesInGalleySpace, EntityType.FACE));
                    try{
                        opExtrude(
                            context,
                            idOfTextExtrude,
                            {
                                //entities:  sheetBodiesInGalleySpace,
                                entities:  qOwnedByBody(sheetBodiesInGalleySpace, EntityType.FACE),
                                direction: vector(0,0,1),
                                endBound: BoundingType.BLIND,
                                endDepth: 0.5 * millimeter,
                                startBound: BoundingType.BLIND,
                                startDepth: zeroLength
                            }
                        );
                    } catch (error)
                    {
                        println("getFeatureError(context, idOfTextExtrude): " ~ getFeatureError(context, idOfTextExtrude));		// getFeatureError(context, idOfTextExtrude);
                    }
                    
                    
                    
                    var textSolidBodies = qBodyType(qCreatedBy(idOfTextExtrude, EntityType.BODY), BodyType.SOLID);
                    //debug(context, textSolidBodies);                    
                    //debug(context, sheetBodiesInGalleySpace);
                    //debug(context, galleyMask);
                    //println("before clipping: reportOnEntities(context, textSolidBodies): " ~ reportOnEntities(context, textSolidBodies,0,0));		
                    //println("before clipping: reportOnEntities(context, galleyMask): " ~ reportOnEntities(context, galleyMask,0,0));
                    
                    if(false){ //This doesn't work because the boolean intersection completely ignores the "targets" argument.
                        // It acts only on the tools.
                        opBoolean(context, idOfClipping,
                            {
                                tools: galleyMask,
                                targets: textSolidBodies,
                                ////targets: sheetBodiesInGalleySpace,
                                operationType: BooleanOperationType.INTERSECTION,
                                targetsAndToolsNeedGrouping:true,
                                keepTools:true
                            }
                        ); 
                    }
                    
                    
                    opBoolean(
                        context,
                        idOfClipping,
                        {
                            tools: galleyMask,
                            targets: textSolidBodies,
                            //targets: sheetBodiesInGalleySpace,
                            operationType: BooleanOperationType.SUBTRACT_COMPLEMENT,
                            targetsAndToolsNeedGrouping:false,
                            keepTools:false
                        }
                    );
                    // // Counter-intuitively, the boolean SUBTRACT_COMPLEMENT operation (which relies on the boolean SUBTRACT operation 
                    // // under the hood,
                    // // and therefore this is probably also true for the SUBTRACT operation) essentially destroys all input bodies 
                    // // and creates brand new bodies.  Therefore, we need to redefine the textSoidBodies query
                    // // to be the set of solid bodies created by the clipping operation:
                    // // textSolidBodies = qBodyType(qCreatedBy(idOfClipping, EntityType.BODY), BodyType.SOLID);
                    // UPDATE: after updating from Featurescript version 626 to version 1271,
                    // it seems that the boolean SUBTRAC_COMPLEMENT operation (and, presumably also the SUBTRACT operation)
                    // now behaves intuitively and does not destroy all input bodies.  Therefore, it is no longer necessary
                    // to redefine the textSolidBodies query to be the set of solid boides created by the clipping operation.
                    // In fact, if we did now perform that re-definition, the newly defined textSolidBodies query would 
                    // resolve to nothing, because the new version of the SUBTRACt_COMPLEMENT operation does not 'create' any solid
                    // bodies - it merely modifies them.  (although perhaps it does create edges and faces where existing solid bodies are chopped
                    // up.
            
                    

                    
                    
                    
                    
                    //println("after clipping: reportOnEntities(context, textSolidBodies): " ~ reportOnEntities(context, textSolidBodies,0,0));		
                    //println("after clipping: reportOnEntities(context, galleyMask): " ~ reportOnEntities(context, galleyMask,0,0));
                    //debug(context, qOwnedByBody(textSolidBodies, EntityType.EDGE));  
                    var allFacesOfTextSolidBodies = qOwnedByBody(textSolidBodies,EntityType.FACE);
                    var facesToKeep = qCoincidesWithPlane(allFacesOfTextSolidBodies, XY_PLANE);
                    var facesToDelete = qSubtraction(allFacesOfTextSolidBodies, facesToKeep);
                    var newEntitiesFromDeleteFace = startTracking(context, qUnion([textSolidBodies, allFacesOfTextSolidBodies]));
                    
                    //delete faces from allFacesOfTextSolidBodies that do not lie on the XY plane
                    var idOfDeleteFace = uniqueId(context, id);
                    try silent{
                        opDeleteFace(
                            context,
                            idOfDeleteFace,
                            {
                                deleteFaces: facesToDelete ,
                                includeFillet:false,
                                capVoid:false,
                                leaveOpen:true
                            }
                        );
                        // this opDeleteFace will throw an excpetion when facesToDelete is empty (which happens when all the textSolidBodies lie entirely outside the galley mask.  That is the reason for the try{}.
                        
                    } catch(error)
                    {
                        
                    }
                    //by deleting faces, the solid bodies will have become sheet bodies.
                    // the opDeleteFace operation doesn't "create" any bodies (in the sense of OnShape id assignment), however it does seem to destroy all input bodies (at least in this case, where we are removing faces from a solid body to end up with a sheet body).  The only way I have found to retrieve the resultant sheet bodies is with a tracking query.  
                    var clippedSheetBodiesInGalleySpace =
                        qBodyType(
                            qOwnerBody(
                                qEntityFilter(newEntitiesFromDeleteFace,EntityType.FACE)
                            ), 
                            BodyType.SHEET
                        );
                    
                    //Not knowing exactly how the tracking query works, I am running the query through evaluateQuery() here for good measure, to make sure that I can use this query later on to still refer to preciesly the entities which exist at this point in the build history.            
                    clippedSheetBodiesInGalleySpace = qUnion(evaluateQuery(context, clippedSheetBodiesInGalleySpace));
    
                    if(false){
                        println("reportOnEntities(context, qCreatedBy(idOfDeleteFace),1,0): "      ~ "\n" ~ reportOnEntities(context, qCreatedBy(idOfDeleteFace),   1, 0));	
                        println("reportOnEntities(context, textSolidBodies,1,0): "                 ~ "\n" ~ reportOnEntities(context, textSolidBodies,              1, 0));	
                        println("reportOnEntities(context, facesToKeep,1,0): "                     ~ "\n" ~ reportOnEntities(context, facesToKeep,                  1, 0));	
                        println("reportOnEntities(context, newEntitiesFromDeleteFace,0,0): "       ~ "\n" ~ reportOnEntities(context, newEntitiesFromDeleteFace,    1, 0));	
                        println("reportOnEntities(context, clippedSheetBodiesInGalleySpace, 1, 0): "      ~ "\n" ~ reportOnEntities(context, clippedSheetBodiesInGalleySpace,     1, 0)); 
                        //debug(context,clippedSheetBodiesInGalleySpace);
                        //debug(context,sheetBodiesInGalleySpace);
                    }
                    
                    //delete the original sheetBodiesInGalleySpace, and set sheetBodiesInGalleySpace = clippedSheetBodiesInGalleySpace
                    opDeleteBodies(context, uniqueId(context, id),{entities: sheetBodiesInGalleySpace});
                    sheetBodiesInGalleySpace = clippedSheetBodiesInGalleySpace;
            
                    
                    
                }
                //println("reportOnEntities(context,sheetBodiesInGalleySpace,0): " ~ toString(reportOnEntities(context,sheetBodiesInGalleySpace,0)));
                 return sheetBodiesInGalleySpace;
            };
        
    
    this[].buildSheetBodiesInWorld = 
        function(context is Context, id is Id)
        {
            var sheetBodiesInWorld = qNothing(); 
            var scaledAnchorPoint;
            
            //anchorPointInGalleySpace is the point in galley space that will be mapped to the origin of worldPlane.
            var anchorPointInGalleySpace;
            // compute anchorPointInGalleySpace. 
            if (is3dLengthVector(this[].anchor))
            {
                anchorPointInGalleySpace =  this[].anchor;
            } else
            {            
                //compute scaledAnchorPoint, one way or another.
                if (isUnitlessVector(this[].anchor) && size(this[].anchor) >= 2)
                {
                    scaledAnchorPoint = resize(this[].anchor, 3, 0); //doing this resize lets us take an anchor that only gives x and y coordinates
                } else if(this[].anchor is galleyAnchor_e)
                {
                    scaledAnchorPoint  = 
                        {
                            galleyAnchor_e.TOP_LEFT:         vector(  0,    1,    0  ),     
                            galleyAnchor_e.TOP_CENTER:       vector(  1/2,  1,    0  ),     
                            galleyAnchor_e.TOP_RIGHT:        vector(  1,    1,    0  ),
                            galleyAnchor_e.CENTER_LEFT:      vector(  0,    1/2,  0  ),  
                            galleyAnchor_e.CENTER:           vector(  1/2,  1/2,  0  ),         
                            galleyAnchor_e.CENTER_RIGHT:     vector(  1,    1/2,  0  ),
                            galleyAnchor_e.BOTTOM_LEFT:      vector(  0,    0,    0  ),  
                            galleyAnchor_e.BOTTOM_CENTER:    vector(  1/2,  0,    0  ),  
                            galleyAnchor_e.BOTTOM_RIGHT:     vector(  1,    0,    0  )
                        }[this[].anchor];
                 } else {
                    throw ("anchor was neither a 3dLengthVector, nor a unitless vector containing at least two elements, nor a galleyAnchor_e enum value.");    
                 }
                 //at this point, scaledAnchorPoint is computed.
                 anchorPointInGalleySpace = elementWiseProduct(scaledAnchorPoint, vector(this[].width, this[].height, zeroLength));
            }

            //println("anchorPointInGalleySpace: " ~ toString(anchorPointInGalleySpace));		// anchorPointInGalleySpace
            
            
            var sheetBodiesInGalleySpace = this[].buildSheetBodiesInGalleySpace(context, uniqueId(context, id));
            //println("reportOnEntities(context,sheetBodiesInGalleySpace,0): " ~ toString(reportOnEntities(context,sheetBodiesInGalleySpace,0)));
            //debug(context, sheetBodiesInGalleySpace);
            opTransform(
                context, 
                uniqueId(context, id), 
                {
                    "bodies": sheetBodiesInGalleySpace,
                    "transform": transform(XY_PLANE, this[].worldPlane) * transform(-anchorPointInGalleySpace)
                }
            );
            sheetBodiesInWorld = sheetBodiesInGalleySpace;
            //debug(context, sheetBodiesInWorld);
            //println("reportOnEntities(context,sheetBodiesInWorld,0,0): " ~ toString(reportOnEntities(context,sheetBodiesInWorld,0,0)));		// reportOnEntities(context,sheetBodiesInWorld,0,0);
            return sheetBodiesInWorld;
        };
        
    addDefaultGettersAndSetters(this); //this gives access to members that are already public via get_...() and set_...() functions, for convenience and consistency of syntax.
    return this as galley;   
}


// takes one or more sheetBodies, and modifies them by deleting the even-rank faces.  This word "rank" here is my own usage, not from the 
// the OnShape documentation.  
function deleteEvenRankFaces(context is Context, id is Id, sheetBodies is Query)
precondition
{
    // all of the below gobbledygook is equivalent to saying "each entity returned by the 'sheetBodies' argument is a sheetBody"
    !queryReturnsSomething(
        context,
        qSubtraction(
            sheetBodies,
            qBodyType(qEntityFilter(sheetBodies, EntityType.BODY), BodyType.SHEET)
        )
    );
}
{
    var tabLevel = 0;
    var i = 0;
    for(var sheetBody in evaluateQuery(context, sheetBodies))
    {
        //remember, a sheetBody may consist of more than one disjoint region, so this algorithm ought to work in that case.
        //println(str_repeat("\t", tabLevel) ~ "Now processing sheetBody " ~ (i + 1) ~ ".");
        tabLevel += 1;
        
        //partition remainingFaces into evenRankFaces and oddRankFaces.
        var evenRankFaces = qNothing();
        var oddRankFaces = qNothing();
        var remainingFaces = qEntityFilter(qOwnedByBody(sheetBody),EntityType.FACE);

        var maxAllowedRank = 100; //the purpose of having this maxAllowedRank is as a way to avoid an infinite loop if I screw up the code somehow.  (OnShape seizes up (in the cloud) for a 10-20 minutes when you tell it to run an infinite loop.)
        for(var rank = 1; queryReturnsSomething(context, remainingFaces) && rank<=maxAllowedRank; rank+=1)
        {
            //println(str_repeat("\t", tabLevel) ~ "Now processing rank " ~ rank);
            tabLevel += 1;
            var allEdges = qEntityFilter(
                //qEdgeAdjacent(remainingFaces ,EntityType.EDGE) , 
                qAdjacent(remainingFaces, AdjacencyType.EDGE, EntityType.EDGE),
                EntityType.EDGE
            ); //all the edges that belong to any of the remainingFaces.  (The qEntityFilter(... , EntityType.EDGE) is probably unnecessary -- I just want to be absolutely sure that I am getting only edges.
            // I am using the word "laminar" here (in this block of code) in a slightly nonstandard way.  In the standard usage, a laminar edge is an edge that belongs to exactly one face among all the faces in a body.  Here, I am using laminar edge to mean an edge that belongs to exactly one face among all the faces in remainingFaces.

            var laminarEdges = 
                qUnion(
                    filter(
                        evaluateQuery(context,allEdges),
                        function(thisEdge)
                        {
                            return 
                                queryReturnsExactlyOneEntity(
                                    context,  
                                    qIntersection (
                                        [
                                            //qEdgeAdjacent(thisEdge, EntityType.FACE), //all the faces which thisEdge belongs to
                                            qAdjacent(thisEdge, AdjacencyType.EDGE, EntityType.FACE),
                                            remainingFaces
                                        ]
                                    ) //this query returns all faces from among the remainingFaces that this edge belongs to.
                                );
                        }    
                    )
                );
            //laminar edges is now a query that returns all the edges that belong to exactly one face from among the remainingFaces.
            
            var outerFaces = 
                qIntersection(
                    [
                        //qEdgeAdjacent(laminarEdges ,EntityType.FACE),
                        qAdjacent(laminarEdges, AdjacencyType.EDGE, EntityType.FACE),
                        remainingFaces
                    ]
                ); //returns all the faces (among the remainingFaces) that any of the laminarEdges belong to.
            
            
            //println(str_repeat("\t", tabLevel) ~ "numberOfEntitiesReturnedByQuery(context, remainingFaces): " ~ numberOfEntitiesReturnedByQuery(context, remainingFaces));		// numberOfEntitiesReturnedByQuery(context, remainingFaces);
            //println(str_repeat("\t", tabLevel) ~ "numberOfEntitiesReturnedByQuery(context, allEdges): " ~ numberOfEntitiesReturnedByQuery(context, allEdges));		// numberOfEntitiesReturnedByQuery(context, allEdges);
            //println(str_repeat("\t", tabLevel) ~ "numberOfEntitiesReturnedByQuery(context, laminarEdges): " ~ numberOfEntitiesReturnedByQuery(context, laminarEdges));		// numberOfEntitiesReturnedByQuery(context, laminarEdges);
            //println(str_repeat("\t", tabLevel) ~ "numberOfEntitiesReturnedByQuery(context, outerFaces): " ~ numberOfEntitiesReturnedByQuery(context, outerFaces));		// numberOfEntitiesReturnedByQuery(context, outerFaces);
            
            
            if(isEven(rank))
            {
               //in this case, the rank (of all of the outerFaces) is even, so we will append all of the outerFaces to evenRankFaces.
               //println(str_repeat("\t", tabLevel) ~ "rank is even");
               evenRankFaces = qUnion([evenRankFaces, outerFaces]); 
            } else 
            {
                //in this case, the rank (of all of the outerFaces) is odd, so we will append all of the outerFaces to oddRankFaces.
                //println(str_repeat("\t", tabLevel) ~ "rank is odd");
                oddRankFaces = qUnion([oddRankFaces, outerFaces]);  
            }
            
        
            
            //now that we have classified the outerFaces for this iteration (i.e. we have put the outerFaces into either evenRankFaces or oddRankFaces (thinking of a query like a collection, from which we can add (via qUnion()) and remove (via qSubtraction()) entities.)), we remove outerFaces from remainingFaces.
            remainingFaces = qSubtraction(remainingFaces, outerFaces);
            
            tabLevel -= 1;
            if(rank == maxAllowedRank)
            {
                throw ("rank reached maxRank (" ~ maxAllowedRank ~ ") -- there could be bug in the rank-partitioning loop that is causing an infinite loop ('loop' in the computer science sense, not the topological sense).") ; 
            }
        }
        
        var idOfOpDeleteFace = id + "opDeleteFace" + i;
            
        if(queryReturnsSomething(context, evenRankFaces))
        {
            try
            {
                    
                opDeleteFace(context, idOfOpDeleteFace,
                    {
                        "deleteFaces":evenRankFaces ,
                        "includeFillet":false,
                        "capVoid": false,
                        "leaveOpen":true
                    }
                );
            }
        } 
        i+=1;
        
        tabLevel -= 1;
    }
}
