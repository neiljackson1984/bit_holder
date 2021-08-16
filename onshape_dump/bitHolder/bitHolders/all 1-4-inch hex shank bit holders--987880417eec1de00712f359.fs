FeatureScript 1540;
import(path : "onshape/std/geometry.fs", version : "1540.0");

annotation { "Default Units" : ["degree", "kilogram", "millimeter"] }
export function main()
{
    return build({});
}

export function build()
{
    return buildPrivate({});
}

export function build(configuration is map)
{
    return buildPrivate(configuration);
}

const buildPrivate = definePartStudio(function(context is Context, configuration is map, lookup is function)
    precondition
    {
    }
    {
        const id is Id = newId();
        annotation { 'unused' : true }
        var features = {};
        features.FosAhS5AHTnwn4r_1 = function(id)
            {
                annotation { 'unused' : true }
                var features = features;
                if (true)
                {
                    {
                    }
                    var qLYMCpfVrBfhSF_query;
                    qLYMCpfVrBfhSF_query=qCompressed(1.0,"%B5$QueryM4Sa$entityTypeBa$EntityTypeS4$FACESb$historyTypeS8$CREATIONSb$operationIdB2$IdA1S5.7$RightplaneOpS9$queryTypeS5$DUMMY",id);
                    annotation { "Feature Name" : "Plane 1" }
                    cPlane(context, id + "FosAhS5AHTnwn4r_1", { "entities" : qUnion([qLYMCpfVrBfhSF_query]), "cplaneType" : CPlaneType.OFFSET, "offset" : { 'value' : try(5 * millimeter), 'expression' : "5 mm" }.value, "angle" : { 'value' : try(0 * degree), 'expression' : "0 deg" }.value, "oppositeDirection" : false, "flipAlignment" : false, "width" : { 'value' : try(150 * millimeter), 'expression' : "150 mm" }.value, "height" : { 'value' : try(150 * millimeter), 'expression' : "150 mm" }.value, "asVersion" : FeatureScriptVersionNumber.V1324_FLIP_NORMAL });
                }
            };
        try(features.FosAhS5AHTnwn4r_1(id));
        features.Fe3kgTkIQcUex9Y_0 = function(id)
            {
                annotation { 'unused' : true }
                var features = features;
                if (true)
                {
                    {
                    }
                    annotation { "Feature Name" : "#nameOfSelectedCannedDesign" }
                    e9f05a85f95bf68be1c454008::m3fe83b491664242c37fd48a9::bitHolder_feature(context, id + "Fe3kgTkIQcUex9Y_0", { "nameOfSelectedCannedDesign" : "all 1/4-inch hex shank bit holders", "cannedDesignChoices" : [{ "name" : "1/4-inch drive, imperial sockets holder", "displayName" : "( )  1/4-inch drive, imperial sockets holder", "selected" : false }, { "name" : "1/4-inch drive, metric sockets holder", "displayName" : "( )  1/4-inch drive, metric sockets holder", "selected" : false }, { "name" : "1/4-inch hex shank drill bits holder", "displayName" : "( )  1/4-inch hex shank drill bits holder", "selected" : false }, { "name" : "1/4-inch hex shank driver bits holder", "displayName" : "( )  1/4-inch hex shank driver bits holder", "selected" : false }, { "name" : "1/4-inch hex shank long driver bits holder", "displayName" : "( )  1/4-inch hex shank long driver bits holder", "selected" : false }, { "name" : "3/8-inch drive, imperial sockets holder", "displayName" : "( )  3/8-inch drive, imperial sockets holder", "selected" : false }, { "name" : "3/8-inch drive, metric sockets holder", "displayName" : "( )  3/8-inch drive, metric sockets holder", "selected" : false }, { "name" : "all 1/4-inch hex shank bit holders", "displayName" : "(*)  all 1/4-inch hex shank bit holders", "selected" : true }, { "name" : "bondhus_hex_drivers_holder", "displayName" : "( )  bondhus_hex_drivers_holder", "selected" : false }, { "name" : "defaultBitHolder", "displayName" : "( )  defaultBitHolder", "selected" : false }], "refreshCannedDesignsRequested" : false, "changeCount" : { 'value' : try(29.0), 'expression' : "29.0" }.value });
                }
            };
        try(features.Fe3kgTkIQcUex9Y_0(id));
        return context;
    }, millimeter, {});
