/****************************
Uncomment the lines below to put the Sandbox Scripts
in debug mode (enable the debugger statement) and 
to disable the QOS protection
*****************************/
/*
$Sandbox.config.debug = true;
$Sandbox.config.qos = false;
*/
function SilverlightLoaded(sender, obj2) {
    var childNodes = document.getElementById('silverlightControlHost').childNodes;
    for (var x = 0; x < childNodes.length; x++) {
        if (childNodes[x].nodeName != "#text") {
            window.silverlightControlHost = childNodes[x];
            break;
        }
    }
    document.getElementById('RunButton').disabled = false;
}

function GetXmlClient() {
    return window.silverlightControlHost;
}

function autoSelectRB(id) {
    document.getElementById(id).checked = true;
}


/***********************************
GadgetManager Code
***********************************/
document.initializeHTML = function() { };
function GadgetAssetsCreated(gad, Key) {
    MainGadgetManager.gadgets[Key].setInData(gad.inHtml, gad.inCSS, gad.inJS, gad.Source);
}
function GadgetCodeCreated(gad, Key) {
    var gadget = MainGadgetManager.gadgets[Key];
    gadget.setOutData(gad.outHtml, gad.outCSS, gad.outJS, gad.outMeta);
    gadget.initialize();

    //Create Instance
    gadget.createInstance(document.getElementById('outPane'), MainGadgetManager.defaultGadgetPolicy, true);
}
function GadgetLoadedFromWebService(gad) {
    var gadget = MainGadgetManager.gadgets[gad.Key];
    gadget.setInData(gad.inHtml, gad.inCss, gad.inJs, gad.Source);
    gadget.setOutData(gad.jsonHtml, gad.jsonCss, gad.outJS, gad.jsonMeta);
    gadget.initialize();
    //Create Instance
    gadget.createInstance(document.getElementById('outPane'), MainGadgetManager.defaultGadgetPolicy, true);
}
var GadgetStartZIndex = 200;
//Gadget Manager Class
function GadgetManager() {
    var me = this;

    me.gadgetIdGen = 0;
    me.gadgets = {};
    me.defaultGadgetPolicy = $Policy.Gadget;

    //Register For Rules
    $Rule.registerFor($Policy.Gadget.__proxyDocument__, 'title', 'onafterset', function(c, o, m, a, blob) {
        if (blob.title)
            blob.title.innerText = a;
    });
    $Rule.registerFor($Policy.Gadget.__global__, 'defaultStatus', 'onafterset', function(c, o, m, a, blob) {
        if (blob.status)
            blob.status.innerText = a;
    });

    me.CreateGadget = function(Html, Url, Silverlight) {
        var GadgetID = me.gadgetIdGen++;
        me.gadgets[GadgetID] = new Gadget(GadgetID);
        if (Html == null) {
            me.gadgets[GadgetID].preLoad(document.getElementById('outPane'), 'Loading from URL: ' + Url);
            if (Silverlight) {
                var plugin = GetXmlClient();
                plugin.Content.myObject.CreateGadget(null, Url, GadgetID);
            }
            else {
                WebSandbox.GadgetService.CreateGadgetFromUrlSync(Url, GadgetID, GadgetLoadedFromWebService);
            }
        }
        else {
            me.gadgets[GadgetID].preLoad(document.getElementById('outPane'), 'Loading from HTML Source');
            if (Silverlight) {
                var plugin = GetXmlClient();
                plugin.Content.myObject.CreateGadget(Html, null, GadgetID);
            }
            else {
                WebSandbox.GadgetService.CreateGadgetFromHTMLSync(Html, GadgetID, null, GadgetLoadedFromWebService);
            }
        }
    };

    me.createInstance = function(UniqueID) {
        var gad = me.gadgets[UniqueID];
        if (gad) {
            gad.createInstance(document.getElementById('outPane'), gad.defaultPolicy);
        }
    };

    me.removeInstance = function(UniqueID, InstanceID) {
        var gad = me.gadgets[UniqueID];
        if (gad) {
            gad.removeInstance(InstanceID);
        }
    };

    me.reloadInstance = function(UniqueID, InstanceID) {
        var gad = me.gadgets[UniqueID];
        if (gad) {
            gad.reloadInstance(InstanceID);
        }
    };

    me.pauseInstance = function(UniqueID, InstanceID, pause) {
        var gad = me.gadgets[UniqueID];
        if (gad) {
            gad.pauseInstance(InstanceID, pause);
        }
    };

    me.removeAllGadgets = function() {
        for (var gad in me.gadgets) {
            me.gadgets[gad].removeAllInstances();
        }
        me.gadgets = {};
    };
}

var MainGadgetManager = new GadgetManager();

//Gadget Class
function Gadget(UniqueID) {
    var me = this;

    me.UniqueID = UniqueID;

    me.instanceIdGen = 0;

    me.instances = {};

    me.setInData = function(inHtml, inCss, inJS, Source) {
        me.inHtml = inHtml;
        me.inCSS = inCss;
        me.inJS = inJS;
        me.Source = Source;
    };

    me.setOutData = function(outHtml, outCSS, outJS, outMeta) {
        me.outHtml = outHtml;
        me.outCSS = outCSS;
        me.outJS = outJS;
        me.outMeta = outMeta;
    };

    me.initialize = function() {
        var settings = {};
        if (me.outCSS && me.outCSS.length > 0) {
            eval("settings = { css : " + me.outCSS + " };");
        }
        eval("var func = " + me.outJS + ";var meta = " + me.outMeta + ";");
        $Sandbox.registerCode(func, UniqueID, settings, meta);
    };

    me.defaultPolicy = null;

    me.focusOnGadget = function(Gadget) {
        if (Gadget.scrollIntoView) {
            Gadget.scrollIntoView();
            document.documentElement.scrollLeft = document.body.scrollLeft = 0;
        }
    }

    me.preLoad = function(el, title) {
        var InstanceID = me.instanceIdGen++;
        var Template = me.createTemplate(InstanceID, el);

        document.getElementById(Template.id + '_title').innerHTML = title;
        document.getElementById(Template.id + '_pause').style.display = 'none';
        document.getElementById(Template.id + '_clone').style.display = 'none';
        document.getElementById(Template.id + '_reload').style.display = 'none';

        me.focusOnGadget(Template);
    }



    me.createInstance = function(el, Policy, preLoaded) {
        var Template, InstanceID;
        if (preLoaded === true) {
            Template = document.getElementById('g_' + me.UniqueID + '_0');
            if (Template) {
                InstanceID = 0;
                document.getElementById(Template.id + '_pause').style.display = '';
                document.getElementById(Template.id + '_clone').style.display = '';
                document.getElementById(Template.id + '_reload').style.display = '';
            }
            else {
                return;
            }
        }
        else {
            InstanceID = me.instanceIdGen++;
            Template = me.createTemplate(InstanceID, el);
        }


        if (me.defaultPolicy == null)
            me.defaultPolicy = Policy

        //Set Title
        var title = $Sandbox.getCode(me.UniqueID).metaData.title;
        if (!title || title === null)
            title = "";
        document.getElementById(Template.id + '_title').innerHTML = title;

        //Set Data
        document.getElementById(Template.id + '_insource').value = me.Source;

        var outSource = '';
        if (me.outCSS && me.outCSS.length > 0)
            outSource += 'var settings = { css : ' + me.outCSS + ' };\n\n';
        else
            outSource += 'var settings = {};\n\n';

        outSource += 'var headerJavaScript = ' + me.outJS + ';\n\n'

        outSource += 'var metadata = ' + me.outMeta + ';\n\n'

        outSource += '$Sandbox.registerCode(headerJavaScript, "' + me.UniqueID + '", settings, metadata);\n\n';

        //Get Policy Name
        var PolicyName = '$Policy.Test';
        for (var strName in $Sandbox.Policies) {
            if ($Sandbox.Policies[strName] == Policy) {
                PolicyName = strName;
                break;
            }
        }

        outSource += 'var SandboxInstance = new $Sandbox(document.getElementById(\'' + Template.id + '_inst\'), ' + PolicyName + ', "' + me.UniqueID + '");\n\n';

        outSource += 'SandboxInstance.initialize();';

        document.getElementById(Template.id + '_outsource').value = outSource;

        //Create objBlob
        var objBlob = {};
        objBlob.title = document.getElementById(Template.id + '_title');
        objBlob.status = document.getElementById(Template.id + '_status');
        objBlob.state = document.getElementById(Template.id + '_state');

        me.instances[InstanceID] = new $Sandbox(document.getElementById(Template.id + '_inst'), Policy, me.UniqueID, objBlob);

        me.instances[InstanceID].onerror = DoError;
        me.instances[InstanceID].onload = DoLoad;
        me.instances[InstanceID].onstop = DoStop;

        me.instances[InstanceID].initialize();

        if (preLoaded !== true) {
            me.focusOnGadget(Template);
        }


        return me.instances[InstanceID];
    };

    me.createTemplate = function(InstanceID, el) {
        var Template = document.getElementById('gadget_template').cloneNode(true);

        Template.id = 'g_' + me.UniqueID + '_' + InstanceID;

        me.renameIds(Template, InstanceID);

        Template.style.zIndex = --GadgetStartZIndex;
        el.appendChild(Template);

        return Template;
    };

    me.renameIds = function(el, InstanceID) {
        for (var x = 0; x < el.childNodes.length; x++) {
            if (el.childNodes[x].id)
                el.childNodes[x].id = 'g_' + me.UniqueID + '_' + InstanceID + '_' + el.childNodes[x].id;
            me.renameIds(el.childNodes[x], InstanceID);
        }
    };

    me.reloadInstance = function(instance) {
        var inst = me.instances[instance];
        if (inst) {
            inst.reload();
        }
    };

    me.pauseInstance = function(instance, pause) {
        var inst = me.instances[instance];
        if (inst) {
            inst.setDisabled(pause);
        }
    };

    me.removeInstance = function(instance) {
        var inst = me.instances[instance];
        if (inst) {
            inst.remove();
            var TopElement = document.getElementById('g_' + me.UniqueID + '_' + instance);
            TopElement.parentNode.removeChild(TopElement);
            me.instances[instance] = null;
        }
        else {
            var TopElement = document.getElementById('g_' + me.UniqueID + '_' + instance);
            TopElement.parentNode.removeChild(TopElement);
        }
    };

    me.removeAllInstances = function() {
        for (var instance in me.instances) {
            me.removeInstance(instance);
        }
        me.instances = {};
    };
}

//UI Code Access
function createGadget() {
    MainGadgetManager.CreateGadget(document.getElementById('userSource').value, null, true);
}

function clearGadgets() {
    MainGadgetManager.removeAllGadgets();
}

function RemoveInstance(element) {
    var Ids = element.id.split('_');
    MainGadgetManager.removeInstance(Ids[1], Ids[2]);
    return false;
}
function ReloadInstance(element) {
    var Ids = element.id.split('_');
    if (!IsInstancePaused(element))
        MainGadgetManager.reloadInstance(Ids[1], Ids[2]);
    return false;
}
function CreateInstance(element) {
    var Ids = element.id.split('_');
    MainGadgetManager.createInstance(Ids[1], Ids[2]);
    return false;
}

function IsInstancePaused(element) {
    var Ids = element.id.split('_');

    var pause = document.getElementById('g_' + Ids[1] + '_' + Ids[2] + '_results').className !== "disabled";

    return !pause;
}

function PauseInstance(element) {
    var Ids = element.id.split('_');

    var pause = !IsInstancePaused(element);

    MainGadgetManager.pauseInstance(Ids[1], Ids[2], pause);


    if (pause) {
        element.innerText = "[ Resume ]";
        document.getElementById('g_' + Ids[1] + '_' + Ids[2] + '_results').className = "disabled";
        document.getElementById('g_' + Ids[1] + '_' + Ids[2] + '_reload').disabled = true;
    }
    else {
        element.innerText = "[ Pause ]";
        document.getElementById('g_' + Ids[1] + '_' + Ids[2] + '_results').className = "instance";
        document.getElementById('g_' + Ids[1] + '_' + Ids[2] + '_reload').disabled = false;
    }

    element.focus();
    return false;
}

function DoError(obj, objBlob, ex) {
    if (objBlob.state) {
        objBlob.state.className = "state error";
        objBlob.state.title = "Currently Disabled";
    }
}

function DoLoad(obj, objBlob) {
    if (objBlob.state) {
        objBlob.state.className = "state good";
        objBlob.state.title = "Currently Running";
    }
}

function DoStop(obj, objBlob, blnState) {
    if (blnState) {
        if (objBlob.state) {
            objBlob.state.className = "state disable";
            objBlob.state.title = "Currently Paused";
        }
    }
    else {
        if (objBlob.state) {
            objBlob.state.className = "state good";
            objBlob.state.title = "Currently Running";
        }
    }
}
