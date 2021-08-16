//FeatureScript 638;
//import(path : "onshape/std/geometry.fs", version : "638.0");
FeatureScript 1271;
import(path : "onshape/std/geometry.fs", version : "1271.0");

import(path : "05ac46458fd6234d1ad55b80", version : "a3d0469ed33d0bb491d9d921");
import(path : "20be3e9e3b832f6babff03e7", version : "fe25cd47b70ccda1fd60e890");
//import(path : "e74f2213a201f3810593935d", version : "64c079e8e4d0a2c9ff16db51");

//the 'MountHole' type is a simply a bundle of properties describing a MountHole.
export type MountHole typecheck isMountHole;

export predicate isMountHole(value)
{
    value is box; 
}

export function new_MountHole() returns MountHole
{
    var this = new box({});
    this[].shankClearanceDiameter = 3 * millimeter; 
    this[].headClearanceDiameter = 8 * millimeter; 
    this[].headClearanceHeight = 2.7 * millimeter; 
    this[].minimumAllowedClampingThickness = 1/4 * inch; 
    this[].clampingDiameter = 5/8 * inch;
    
    this[].build = 
        function(context is Context, id is Id, definition is map )
        {
            
            //definition.targetBodies is a query specifiying zero or more solid bodies, into which the hole will be cut.
            //definition.axis is the axis along which the hole will be cut.
             // identify the point (counterbore end) on axis that is minimumAllowedClampingThickness away (in the negative axis direction) from the 
            // last point on axis that is contained in any of the targetBodies.  The shankClearance hole will extend from the counterBoreEndPoint to the positive
            // extreme point of the bodies on the axis.  The headClearance hole (a.k.a. counterbore) will extend from counterboreEndPoint to the negative extreme point of the bodies on the axis.
            
            // compute positiveExtremePointOfBodies, the farthest along the axis direction of all the intersection points between the axis and  the target bodies.
            
            //debug(context, evExtremeSkewerPointsOfBodies(context, definition.targetBodies, definition.axis)[0]);
            //debug(context, evExtremeSkewerPointsOfBodies(context, definition.targetBodies, definition.axis)[1]);
            
            var counterBoreEndPoint = evExtremeSkewerPointsOfBodies(context, definition.targetBodies, definition.axis)[1] - this[].get_minimumAllowedClampingThickness() * definition.axis.direction;
            //debug(context, counterBoreEndPoint);
            
            var shankClearanceDiameterSheetBody = 
                createCircleSheetBody(
                    context, 
                    uniqueId(context, id), 
                    {
                        "circle": 
                            circle(
                                planeToCSys(
                                    plane(
                                        counterBoreEndPoint,  //origin
                                        yHat //normal
                                    )
                                ), 
                                this[].get_shankClearanceDiameter()/2
                            )
                    }
                );
                
            var headClearanceDiameterSheetBody = 
                 createCircleSheetBody(
                    context, 
                    uniqueId(context, id), 
                    {
                        "circle": 
                            circle(
                                planeToCSys(
                                    plane(
                                        counterBoreEndPoint,  //origin
                                        yHat //normal
                                    )
                                ), 
                                this[].get_headClearanceDiameter()/2
                            )
                    }
                );
            
            var idOfShankClearanceExtrude = uniqueId(context, id);
            var idOfHeadClearanceExtrude = uniqueId(context, id);
            
            opExtrude(
                context,
                idOfShankClearanceExtrude, 
                {
                    "entities" : qOwnedByBody(shankClearanceDiameterSheetBody, EntityType.FACE),
                    "direction" : evOwnerSketchPlane(context, {"entity" : shankClearanceDiameterSheetBody}).normal,
                    "endBound" : BoundingType.THROUGH_ALL,
                }
            );
            
            opExtrude(
                context,
                idOfHeadClearanceExtrude, 
                {
                    "entities" : qOwnedByBody(headClearanceDiameterSheetBody, EntityType.FACE),
                    "direction" : -evOwnerSketchPlane(context, {"entity" : headClearanceDiameterSheetBody}).normal,
                    "endBound" : BoundingType.THROUGH_ALL,
                }
            );
            
            var idOfBoolean = uniqueId(context, id);
            
            opBoolean(
                context,
                idOfBoolean,
                {
                    tools: 
                        qUnion(
                            [
                                qCreatedBy(idOfShankClearanceExtrude,  EntityType.BODY),
                                qCreatedBy(idOfHeadClearanceExtrude,   EntityType.BODY)
                            ]
                        ),
                    targets: definition.targetBodies,
                    operationType: BooleanOperationType.SUBTRACTION,
                    targetsAndToolsNeedGrouping:true
                }
            );
            
            var bodiesToDelete = qUnion([shankClearanceDiameterSheetBody, headClearanceDiameterSheetBody]);
            
            //print("bodiesToDelete: ");  debug(context, bodiesToDelete);
            // println("reportOnEntities(context, qCreatedBy(idOfShankClearanceExtrude),0,0): " ~ toString(reportOnEntities(context, qCreatedBy(idOfShankClearanceExtrude),0,0)));		// reportOnEntities(context, qCreatedBy(idOfShankClearanceExtrude),0,0);
            
            opDeleteBodies(context, uniqueId(context, id), {entities: bodiesToDelete});
            
        };
    
    addDefaultGettersAndSetters(this);//this gives access to members that are already public via get_...() and set_...() functions, for convenience and consistency of syntax.
    
    return this as MountHole;
}

export function new_MountHole(initialSettings) returns MountHole
{
    var this = new_MountHole();
    this[].set(initialSettings);
    return this as MountHole;
}
