/*
Copyright (c) 2019 Integrative Software LLC
Created: 5/2019
Author: Pablo Carbonell
*/
var LaraUI;
(function (LaraUI) {
    var ContentNodeType;
    (function (ContentNodeType) {
        ContentNodeType[ContentNodeType["Element"] = 1] = "Element";
        ContentNodeType[ContentNodeType["Text"] = 2] = "Text";
    })(ContentNodeType = LaraUI.ContentNodeType || (LaraUI.ContentNodeType = {}));
})(LaraUI || (LaraUI = {}));
/*
Copyright (c) 2019 Integrative Software LLC
Created: 5/2019
Author: Pablo Carbonell
*/
var LaraUI;
(function (LaraUI) {
    var DeltaType;
    (function (DeltaType) {
        DeltaType[DeltaType["Append"] = 1] = "Append";
        DeltaType[DeltaType["Insert"] = 2] = "Insert";
        DeltaType[DeltaType["TextModified"] = 3] = "TextModified";
        DeltaType[DeltaType["Remove"] = 4] = "Remove";
        DeltaType[DeltaType["EditAttribute"] = 5] = "EditAttribute";
        DeltaType[DeltaType["RemoveAttribute"] = 6] = "RemoveAttribute";
        DeltaType[DeltaType["Focus"] = 7] = "Focus";
        DeltaType[DeltaType["SetId"] = 8] = "SetId";
        DeltaType[DeltaType["SetValue"] = 9] = "SetValue";
        DeltaType[DeltaType["SubmitJS"] = 10] = "SubmitJS";
        DeltaType[DeltaType["SetChecked"] = 11] = "SetChecked";
    })(DeltaType = LaraUI.DeltaType || (LaraUI.DeltaType = {}));
})(LaraUI || (LaraUI = {}));
/*
Copyright (c) 2019 Integrative Software LLC
Created: 5/2019
Author: Pablo Carbonell
*/
var LaraUI;
(function (LaraUI) {
    var ElementEventValue = /** @class */ (function () {
        function ElementEventValue() {
        }
        return ElementEventValue;
    }());
    LaraUI.ElementEventValue = ElementEventValue;
    var ClientEventMessage = /** @class */ (function () {
        function ClientEventMessage() {
        }
        ClientEventMessage.prototype.isEmpty = function () {
            return this.Values.length == 0;
        };
        return ClientEventMessage;
    }());
    LaraUI.ClientEventMessage = ClientEventMessage;
    function collectValues() {
        var message = new ClientEventMessage();
        message.Values = [];
        collectType("input", message, collectInput);
        collectType("textarea", message, collectSimpleValue);
        collectType("button", message, collectSimpleValue);
        collectType("select", message, collectSimpleValue);
        collectType("option", message, collectOption);
        return message;
    }
    LaraUI.collectValues = collectValues;
    function collectType(tagName, message, processor) {
        var list = document.getElementsByTagName(tagName);
        for (var index = 0; index < list.length; index++) {
            var el = list[index];
            if (el.id) {
                var entry = new ElementEventValue();
                entry.ElementId = el.id;
                processor(el, entry);
                message.Values.push(entry);
            }
        }
    }
    function collectInput(el, entry) {
        var input = el;
        entry.Value = input.value;
        entry.Checked = input.checked;
    }
    function collectSimpleValue(el, entry) {
        var input = el;
        entry.Value = input.value;
    }
    function collectOption(el, entry) {
        var option = el;
        entry.Checked = option.selected;
    }
})(LaraUI || (LaraUI = {}));
/*
Copyright (c) 2019 Integrative Software LLC
Created: 5/2019
Author: Pablo Carbonell
*/
/*
Lara is a server-side DOM rendering library for C#
This file is the client runtime for Lara.
https://laraui.com
*/
var LaraUI;
(function (LaraUI) {
    var documentId;
    function initialize(id) {
        documentId = id;
        window.addEventListener("unload", terminate, false);
        clean(document);
    }
    LaraUI.initialize = initialize;
    function terminate() {
        var url = "/_discard?doc=" + documentId;
        navigator.sendBeacon(url);
    }
    function plug(el, eventName) {
        var url = getEventUrl(el, eventName);
        sendAjax(url);
    }
    LaraUI.plug = plug;
    function getEventUrl(el, eventName) {
        return "/_event?doc=" + documentId
            + "&el=" + el.id
            + "&ev=" + eventName;
    }
    function sendAjax(url) {
        var ajax = new XMLHttpRequest();
        ajax.onreadystatechange = function () {
            if (this.readyState == 4) {
                if (this.status == 200) {
                    processAjaxResult(this);
                }
                else {
                    processAjaxError(this);
                }
            }
        };
        var message = LaraUI.collectValues();
        ajax.open("POST", url, true);
        if (message.isEmpty()) {
            ajax.send();
        }
        else {
            ajax.send(JSON.stringify(message));
        }
    }
    function processAjaxResult(ajax) {
        var result = JSON.parse(ajax.responseText);
        if (result.List) {
            LaraUI.processResult(result.List);
        }
    }
    function processAjaxError(ajax) {
        if (ajax.responseText) {
            document.write(ajax.responseText);
        }
        else {
            console.log("Internal Server Error on AJAX call. Detailed exception information on the client is turned off.");
        }
    }
    function clean(node) {
        for (var n = 0; n < node.childNodes.length; n++) {
            var child = node.childNodes[n];
            if (child.nodeType === 8
                ||
                    (child.nodeType === 3 && !/\S/.test(child.nodeValue))) {
                node.removeChild(child);
                n--;
            }
            else if (child.nodeType === 1) {
                clean(child);
            }
        }
    }
})(LaraUI || (LaraUI = {}));
/*
Copyright (c) 2019 Integrative Software LLC
Created: 5/2019
Author: Pablo Carbonell
*/
var LaraUI;
(function (LaraUI) {
    function processResult(steps) {
        for (var _i = 0, steps_1 = steps; _i < steps_1.length; _i++) {
            var step = steps_1[_i];
            processStep(step);
        }
    }
    LaraUI.processResult = processResult;
    function processStep(step) {
        switch (step.Type) {
            case LaraUI.DeltaType.Append:
                append(step);
                break;
            case LaraUI.DeltaType.Insert:
                insert(step);
                break;
            case LaraUI.DeltaType.TextModified:
                textModified(step);
                break;
            case LaraUI.DeltaType.Remove:
                remove(step);
                break;
            case LaraUI.DeltaType.EditAttribute:
                editAttribute(step);
                break;
            case LaraUI.DeltaType.RemoveAttribute:
                removeAttribute(step);
                break;
            case LaraUI.DeltaType.Focus:
                focus(step);
                break;
            case LaraUI.DeltaType.SetId:
                setId(step);
                break;
            case LaraUI.DeltaType.SetValue:
                setValue(step);
                break;
            case LaraUI.DeltaType.SubmitJS:
                submitJS(step);
                break;
            case LaraUI.DeltaType.SetChecked:
                setChecked(step);
                break;
            default:
                console.log("Error processing event response. Unknown step type: " + step.Type);
        }
    }
    function append(delta) {
        var el = document.getElementById(delta.ParentId);
        var child = createNode(delta.NodeAdded);
        el.appendChild(child);
    }
    function insert(delta) {
        var el = document.getElementById(delta.ParentElementId);
        var child = createNode(delta.ContentNode);
        if (delta.Index < el.childNodes.length) {
            var before = el.childNodes[delta.Index];
            el.insertBefore(child, before);
        }
        else {
            el.appendChild(child);
        }
    }
    function createNode(node) {
        if (node.Type == LaraUI.ContentNodeType.Text) {
            return createTextNode(node);
        }
        else if (node.Type == LaraUI.ContentNodeType.Element) {
            return createElementNode(node);
        }
        else {
            console.log("Error processing event response. Unknown content type: " + node.Type);
            document.createTextNode("");
        }
    }
    function createTextNode(node) {
        return document.createTextNode(node.Data);
    }
    function createElementNode(node) {
        var child = document.createElement(node.TagName);
        for (var _i = 0, _a = node.Attributes; _i < _a.length; _i++) {
            var attribute = _a[_i];
            child.setAttribute(attribute.Attribute, attribute.Value);
        }
        for (var _b = 0, _c = node.Children; _b < _c.length; _b++) {
            var branch = _c[_b];
            child.appendChild(createNode(branch));
        }
        return child;
    }
    function textModified(delta) {
        var el = document.getElementById(delta.ParentElementId);
        var child = el.childNodes[delta.ChildNodeIndex];
        child.textContent = delta.Text;
    }
    function remove(delta) {
        var parent = document.getElementById(delta.ParentId);
        var child = parent.childNodes[delta.ChildIndex];
        child.remove();
    }
    function editAttribute(delta) {
        var el = document.getElementById(delta.ElementId);
        if (el.tagName == "OPTION" && delta.Attribute == "selected") {
            var option = el;
            option.selected = true;
        }
        else {
            el.setAttribute(delta.Attribute, delta.Value);
        }
    }
    function removeAttribute(delta) {
        var el = document.getElementById(delta.ElementId);
        if (el.tagName == "OPTION" && delta.Attribute == "selected") {
            var option = el;
            option.selected = false;
        }
        else {
            el.removeAttribute(delta.Attribute);
        }
    }
    function focus(delta) {
        var el = document.getElementById(delta.ElementId);
        el.focus();
    }
    function setId(delta) {
        var el = resolveElement(delta.Locator);
        el.id = delta.NewId;
    }
    function resolveElement(locator) {
        var el = document.getElementById(locator.StartingId);
        for (var index = locator.Steps.length - 1; index >= 0; index--) {
            var step = locator.Steps[index];
            el = el.children[step];
        }
        return el;
    }
    function setValue(delta) {
        var input = document.getElementById(delta.ElementId);
        input.value = delta.Value;
    }
    function submitJS(delta) {
        try {
            eval(delta.Code);
        }
        catch (e) {
            console.log(e.message);
        }
    }
    function setChecked(delta) {
        var input = document.getElementById(delta.ElementId);
        input.checked = delta.Checked;
    }
})(LaraUI || (LaraUI = {}));
