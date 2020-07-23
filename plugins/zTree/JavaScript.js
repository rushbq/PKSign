﻿/*
 * JQuery zTree core 3.5.12
 * http://zTree.me/
 *
 * Copyright (c) 2010 Hunter.z
 *
 * Licensed same as jquery - MIT License
 * http://www.opensource.org/licenses/mit-license.php
 *
 * email: hunter.z@263.net
 * Date: 2013-03-11
 */
(function (k) {
    var E, F, G, H, I, J, r = {}, K = {}, s = {}, L = {
        treeId: "", treeObj: null, view: { addDiyDom: null, autoCancelSelected: !0, dblClickExpand: !0, expandSpeed: "fast", fontCss: {}, nameIsHTML: !1, selectedMulti: !0, showIcon: !0, showLine: !0, showTitle: !0 }, data: { key: { children: "children", name: "label", title: "", url: "url" }, simpleData: { enable: !1, idKey: "id", pIdKey: "parentID", rootPId: null }, keep: { parent: !1, leaf: !1 } }, async: {
            enable: !1, contentType: "application/x-www-form-urlencoded", type: "post", dataType: "text", url: "", autoParam: [], otherParam: [],
            dataFilter: null
        }, callback: { beforeAsync: null, beforeClick: null, beforeDblClick: null, beforeRightClick: null, beforeMouseDown: null, beforeMouseUp: null, beforeExpand: null, beforeCollapse: null, beforeRemove: null, onAsyncError: null, onAsyncSuccess: null, onNodeCreated: null, onClick: null, onDblClick: null, onRightClick: null, onMouseDown: null, onMouseUp: null, onExpand: null, onCollapse: null, onRemove: null }
    }, t = [function (b) {
        var a = b.treeObj, c = e.event; a.bind(c.NODECREATED, function (a, c, g) { j.apply(b.callback.onNodeCreated, [a, c, g]) });
        a.bind(c.CLICK, function (a, c, g, l, h) { j.apply(b.callback.onClick, [c, g, l, h]) }); a.bind(c.EXPAND, function (a, c, g) { j.apply(b.callback.onExpand, [a, c, g]) }); a.bind(c.COLLAPSE, function (a, c, g) { j.apply(b.callback.onCollapse, [a, c, g]) }); a.bind(c.ASYNC_SUCCESS, function (a, c, g, l) { j.apply(b.callback.onAsyncSuccess, [a, c, g, l]) }); a.bind(c.ASYNC_ERROR, function (a, c, g, l, h, e) { j.apply(b.callback.onAsyncError, [a, c, g, l, h, e]) })
    }], u = [function (b) { var a = e.event; b.treeObj.unbind(a.NODECREATED).unbind(a.CLICK).unbind(a.EXPAND).unbind(a.COLLAPSE).unbind(a.ASYNC_SUCCESS).unbind(a.ASYNC_ERROR) }],
    v = [function (b) { var a = h.getCache(b); a || (a = {}, h.setCache(b, a)); a.nodes = []; a.doms = [] }], w = [function (b, a, c, d, f, g) {
        if (c) {
            var l = h.getRoot(b), e = b.data.key.children; c.level = a; c.tId = b.treeId + "_" + ++l.zId; c.parentTId = d ? d.tId : null; if (c[e] && c[e].length > 0) { if (typeof c.open == "string") c.open = j.eqs(c.open, "true"); c.open = !!c.open; c.isParent = !0; c.zAsync = !0 } else { c.open = !1; if (typeof c.isParent == "string") c.isParent = j.eqs(c.isParent, "true"); c.isParent = !!c.isParent; c.zAsync = !c.isParent } c.isFirstNode = f; c.isLastNode = g; c.getParentNode =
            function () { return h.getNodeCache(b, c.parentTId) }; c.getPreNode = function () { return h.getPreNode(b, c) }; c.getNextNode = function () { return h.getNextNode(b, c) }; c.isAjaxing = !1; h.fixPIdKeyValue(b, c)
        }
    }], x = [function (b) {
        var a = b.target, c = h.getSetting(b.data.treeId), d = "", f = null, g = "", l = "", i = null, o = null, p = null; if (j.eqs(b.type, "mousedown")) l = "mousedown"; else if (j.eqs(b.type, "mouseup")) l = "mouseup"; else if (j.eqs(b.type, "contextmenu")) l = "contextmenu"; else if (j.eqs(b.type, "click")) if (j.eqs(a.tagName, "span") && a.getAttribute("treeNode" +
        e.id.SWITCH) !== null) d = (k(a).parent("li").get(0) || k(a).parentsUntil("li").parent().get(0)).id, g = "switchNode"; else { if (p = j.getMDom(c, a, [{ tagName: "a", attrName: "treeNode" + e.id.A }])) d = (k(p).parent("li").get(0) || k(p).parentsUntil("li").parent().get(0)).id, g = "clickNode" } else if (j.eqs(b.type, "dblclick") && (l = "dblclick", p = j.getMDom(c, a, [{ tagName: "a", attrName: "treeNode" + e.id.A }]))) d = (k(p).parent("li").get(0) || k(p).parentsUntil("li").parent().get(0)).id, g = "switchNode"; if (l.length > 0 && d.length == 0 && (p = j.getMDom(c,
        a, [{ tagName: "a", attrName: "treeNode" + e.id.A }]))) d = (k(p).parent("li").get(0) || k(p).parentsUntil("li").parent().get(0)).id; if (d.length > 0) switch (f = h.getNodeCache(c, d), g) { case "switchNode": f.isParent ? j.eqs(b.type, "click") || j.eqs(b.type, "dblclick") && j.apply(c.view.dblClickExpand, [c.treeId, f], c.view.dblClickExpand) ? i = E : g = "" : g = ""; break; case "clickNode": i = F } switch (l) { case "mousedown": o = G; break; case "mouseup": o = H; break; case "dblclick": o = I; break; case "contextmenu": o = J } return {
            stop: !1, node: f, nodeEventType: g, nodeEventCallback: i,
            treeEventType: l, treeEventCallback: o
        }
    }], y = [function (b) { var a = h.getRoot(b); a || (a = {}, h.setRoot(b, a)); a[b.data.key.children] = []; a.expandTriggerFlag = !1; a.curSelectedList = []; a.noSelection = !0; a.createdNodes = []; a.zId = 0; a._ver = (new Date).getTime() }], z = [], A = [], B = [], C = [], D = [], h = {
        addNodeCache: function (b, a) { h.getCache(b).nodes[h.getNodeCacheId(a.tId)] = a }, getNodeCacheId: function (b) { return b.substring(b.lastIndexOf("_") + 1) }, addAfterA: function (b) { A.push(b) }, addBeforeA: function (b) { z.push(b) }, addInnerAfterA: function (b) { C.push(b) },
        addInnerBeforeA: function (b) { B.push(b) }, addInitBind: function (b) { t.push(b) }, addInitUnBind: function (b) { u.push(b) }, addInitCache: function (b) { v.push(b) }, addInitNode: function (b) { w.push(b) }, addInitProxy: function (b) { x.push(b) }, addInitRoot: function (b) { y.push(b) }, addNodesData: function (b, a, c) { var d = b.data.key.children; a[d] || (a[d] = []); if (a[d].length > 0) a[d][a[d].length - 1].isLastNode = !1, i.setNodeLineIcos(b, a[d][a[d].length - 1]); a.isParent = !0; a[d] = a[d].concat(c) }, addSelectedNode: function (b, a) {
            var c = h.getRoot(b); h.isSelectedNode(b,
            a) || c.curSelectedList.push(a)
        }, addCreatedNode: function (b, a) { (b.callback.onNodeCreated || b.view.addDiyDom) && h.getRoot(b).createdNodes.push(a) }, addZTreeTools: function (b) { D.push(b) }, exSetting: function (b) { k.extend(!0, L, b) }, fixPIdKeyValue: function (b, a) { b.data.simpleData.enable && (a[b.data.simpleData.pIdKey] = a.parentTId ? a.getParentNode()[b.data.simpleData.idKey] : b.data.simpleData.rootPId) }, getAfterA: function (b, a, c) { for (var d = 0, f = A.length; d < f; d++) A[d].apply(this, arguments) }, getBeforeA: function (b, a, c) {
            for (var d =
            0, f = z.length; d < f; d++) z[d].apply(this, arguments)
        }, getInnerAfterA: function (b, a, c) { for (var d = 0, f = C.length; d < f; d++) C[d].apply(this, arguments) }, getInnerBeforeA: function (b, a, c) { for (var d = 0, f = B.length; d < f; d++) B[d].apply(this, arguments) }, getCache: function (b) { return s[b.treeId] }, getNextNode: function (b, a) { if (!a) return null; for (var c = b.data.key.children, d = a.parentTId ? a.getParentNode() : h.getRoot(b), f = 0, g = d[c].length - 1; f <= g; f++) if (d[c][f] === a) return f == g ? null : d[c][f + 1]; return null }, getNodeByParam: function (b, a,
        c, d) { if (!a || !c) return null; for (var f = b.data.key.children, g = 0, l = a.length; g < l; g++) { if (a[g][c] == d) return a[g]; var e = h.getNodeByParam(b, a[g][f], c, d); if (e) return e } return null }, getNodeCache: function (b, a) { if (!a) return null; var c = s[b.treeId].nodes[h.getNodeCacheId(a)]; return c ? c : null }, getNodeName: function (b, a) { return "" + a[b.data.key.name] }, getNodeTitle: function (b, a) { return "" + a[b.data.key.title === "" ? b.data.key.name : b.data.key.title] }, getNodes: function (b) { return h.getRoot(b)[b.data.key.children] }, getNodesByParam: function (b,
        a, c, d) { if (!a || !c) return []; for (var f = b.data.key.children, g = [], l = 0, e = a.length; l < e; l++) a[l][c] == d && g.push(a[l]), g = g.concat(h.getNodesByParam(b, a[l][f], c, d)); return g }, getNodesByParamFuzzy: function (b, a, c, d) { if (!a || !c) return []; for (var f = b.data.key.children, g = [], d = d.toLowerCase(), l = 0, e = a.length; l < e; l++) typeof a[l][c] == "string" && a[l][c].toLowerCase().indexOf(d) > -1 && g.push(a[l]), g = g.concat(h.getNodesByParamFuzzy(b, a[l][f], c, d)); return g }, getNodesByFilter: function (b, a, c, d, f) {
            if (!a) return d ? null : []; for (var g =
            b.data.key.children, e = d ? null : [], i = 0, k = a.length; i < k; i++) { if (j.apply(c, [a[i], f], !1)) { if (d) return a[i]; e.push(a[i]) } var p = h.getNodesByFilter(b, a[i][g], c, d, f); if (d && p) return p; e = d ? p : e.concat(p) } return e
        }, getPreNode: function (b, a) { if (!a) return null; for (var c = b.data.key.children, d = a.parentTId ? a.getParentNode() : h.getRoot(b), f = 0, g = d[c].length; f < g; f++) if (d[c][f] === a) return f == 0 ? null : d[c][f - 1]; return null }, getRoot: function (b) { return b ? K[b.treeId] : null }, getSetting: function (b) { return r[b] }, getSettings: function () { return r },
        getZTreeTools: function (b) { return (b = this.getRoot(this.getSetting(b))) ? b.treeTools : null }, initCache: function (b) { for (var a = 0, c = v.length; a < c; a++) v[a].apply(this, arguments) }, initNode: function (b, a, c, d, f, g) { for (var e = 0, h = w.length; e < h; e++) w[e].apply(this, arguments) }, initRoot: function (b) { for (var a = 0, c = y.length; a < c; a++) y[a].apply(this, arguments) }, isSelectedNode: function (b, a) { for (var c = h.getRoot(b), d = 0, f = c.curSelectedList.length; d < f; d++) if (a === c.curSelectedList[d]) return !0; return !1 }, removeNodeCache: function (b,
        a) { var c = b.data.key.children; if (a[c]) for (var d = 0, f = a[c].length; d < f; d++) arguments.callee(b, a[c][d]); h.getCache(b).nodes[h.getNodeCacheId(a.tId)] = null }, removeSelectedNode: function (b, a) { for (var c = h.getRoot(b), d = 0, f = c.curSelectedList.length; d < f; d++) if (a === c.curSelectedList[d] || !h.getNodeCache(b, c.curSelectedList[d].tId)) c.curSelectedList.splice(d, 1), d--, f-- }, setCache: function (b, a) { s[b.treeId] = a }, setRoot: function (b, a) { K[b.treeId] = a }, setZTreeTools: function (b, a) {
            for (var c = 0, d = D.length; c < d; c++) D[c].apply(this,
            arguments)
        }, transformToArrayFormat: function (b, a) { if (!a) return []; var c = b.data.key.children, d = []; if (j.isArray(a)) for (var f = 0, g = a.length; f < g; f++) d.push(a[f]), a[f][c] && (d = d.concat(h.transformToArrayFormat(b, a[f][c]))); else d.push(a), a[c] && (d = d.concat(h.transformToArrayFormat(b, a[c]))); return d }, transformTozTreeFormat: function (b, a) {
            var c, d, f = b.data.simpleData.idKey, g = b.data.simpleData.pIdKey, e = b.data.key.children; if (!f || f == "" || !a) return []; if (j.isArray(a)) {
                var h = [], i = []; for (c = 0, d = a.length; c < d; c++) i[a[c][f]] =
                a[c]; for (c = 0, d = a.length; c < d; c++) i[a[c][g]] && a[c][f] != a[c][g] ? (i[a[c][g]][e] || (i[a[c][g]][e] = []), i[a[c][g]][e].push(a[c])) : h.push(a[c]); return h
            } else return [a]
        }
    }, m = {
        bindEvent: function (b) { for (var a = 0, c = t.length; a < c; a++) t[a].apply(this, arguments) }, unbindEvent: function (b) { for (var a = 0, c = u.length; a < c; a++) u[a].apply(this, arguments) }, bindTree: function (b) {
            var a = { treeId: b.treeId }, b = b.treeObj; b.bind("selectstart", function (a) { a = a.originalEvent.srcElement.nodeName.toLowerCase(); return a === "input" || a === "textarea" }).css({ "-moz-user-select": "-moz-none" });
            b.bind("click", a, m.proxy); b.bind("dblclick", a, m.proxy); b.bind("mouseover", a, m.proxy); b.bind("mouseout", a, m.proxy); b.bind("mousedown", a, m.proxy); b.bind("mouseup", a, m.proxy); b.bind("contextmenu", a, m.proxy)
        }, unbindTree: function (b) { b.treeObj.unbind("click", m.proxy).unbind("dblclick", m.proxy).unbind("mouseover", m.proxy).unbind("mouseout", m.proxy).unbind("mousedown", m.proxy).unbind("mouseup", m.proxy).unbind("contextmenu", m.proxy) }, doProxy: function (b) {
            for (var a = [], c = 0, d = x.length; c < d; c++) {
                var f = x[c].apply(this,
                arguments); a.push(f); if (f.stop) break
            } return a
        }, proxy: function (b) { var a = h.getSetting(b.data.treeId); if (!j.uCanDo(a, b)) return !0; for (var a = m.doProxy(b), c = !0, d = 0, f = a.length; d < f; d++) { var g = a[d]; g.nodeEventCallback && (c = g.nodeEventCallback.apply(g, [b, g.node]) && c); g.treeEventCallback && (c = g.treeEventCallback.apply(g, [b, g.node]) && c) } return c }
    }; E = function (b, a) {
        var c = h.getSetting(b.data.treeId); if (a.open) { if (j.apply(c.callback.beforeCollapse, [c.treeId, a], !0) == !1) return !0 } else if (j.apply(c.callback.beforeExpand,
        [c.treeId, a], !0) == !1) return !0; h.getRoot(c).expandTriggerFlag = !0; i.switchNode(c, a); return !0
    }; F = function (b, a) { var c = h.getSetting(b.data.treeId), d = c.view.autoCancelSelected && b.ctrlKey && h.isSelectedNode(c, a) ? 0 : c.view.autoCancelSelected && b.ctrlKey && c.view.selectedMulti ? 2 : 1; if (j.apply(c.callback.beforeClick, [c.treeId, a, d], !0) == !1) return !0; d === 0 ? i.cancelPreSelectedNode(c, a) : i.selectNode(c, a, d === 2); c.treeObj.trigger(e.event.CLICK, [b, c.treeId, a, d]); return !0 }; G = function (b, a) {
        var c = h.getSetting(b.data.treeId);
        j.apply(c.callback.beforeMouseDown, [c.treeId, a], !0) && j.apply(c.callback.onMouseDown, [b, c.treeId, a]); return !0
    }; H = function (b, a) { var c = h.getSetting(b.data.treeId); j.apply(c.callback.beforeMouseUp, [c.treeId, a], !0) && j.apply(c.callback.onMouseUp, [b, c.treeId, a]); return !0 }; I = function (b, a) { var c = h.getSetting(b.data.treeId); j.apply(c.callback.beforeDblClick, [c.treeId, a], !0) && j.apply(c.callback.onDblClick, [b, c.treeId, a]); return !0 }; J = function (b, a) {
        var c = h.getSetting(b.data.treeId); j.apply(c.callback.beforeRightClick,
        [c.treeId, a], !0) && j.apply(c.callback.onRightClick, [b, c.treeId, a]); return typeof c.callback.onRightClick != "function"
    }; var j = {
        apply: function (b, a, c) { return typeof b == "function" ? b.apply(M, a ? a : []) : c }, canAsync: function (b, a) { var c = b.data.key.children; return b.async.enable && a && a.isParent && !(a.zAsync || a[c] && a[c].length > 0) }, clone: function (b) {
            if (b === null) return null; var a = b.constructor === Array ? [] : {}, c; for (c in b) a[c] = b[c] instanceof Date ? new Date(b[c].getTime()) : typeof b[c] === "object" ? arguments.callee(b[c]) :
            b[c]; return a
        }, eqs: function (b, a) { return b.toLowerCase() === a.toLowerCase() }, isArray: function (b) { return Object.prototype.toString.apply(b) === "[object Array]" }, getMDom: function (b, a, c) { if (!a) return null; for (; a && a.id !== b.treeId;) { for (var d = 0, f = c.length; a.tagName && d < f; d++) if (j.eqs(a.tagName, c[d].tagName) && a.getAttribute(c[d].attrName) !== null) return a; a = a.parentNode } return null }, uCanDo: function () { return !0 }
    }, i = {
        addNodes: function (b, a, c, d) {
            if (!b.data.keep.leaf || !a || a.isParent) if (j.isArray(c) || (c = [c]), b.data.simpleData.enable &&
            (c = h.transformTozTreeFormat(b, c)), a) { var f = k("#" + a.tId + e.id.SWITCH), g = k("#" + a.tId + e.id.ICON), l = k("#" + a.tId + e.id.UL); if (!a.open) i.replaceSwitchClass(a, f, e.folder.CLOSE), i.replaceIcoClass(a, g, e.folder.CLOSE), a.open = !1, l.css({ display: "none" }); h.addNodesData(b, a, c); i.createNodes(b, a.level + 1, c, a); d || i.expandCollapseParentNode(b, a, !0) } else h.addNodesData(b, h.getRoot(b), c), i.createNodes(b, 0, c, null)
        }, appendNodes: function (b, a, c, d, f, g) {
            if (!c) return []; for (var e = [], j = b.data.key.children, k = 0, p = c.length; k < p; k++) {
                var n =
                c[k]; if (f) { var m = (d ? d : h.getRoot(b))[j].length == c.length && k == 0; h.initNode(b, a, n, d, m, k == c.length - 1, g); h.addNodeCache(b, n) } m = []; n[j] && n[j].length > 0 && (m = i.appendNodes(b, a + 1, n[j], n, f, g && n.open)); g && (i.makeDOMNodeMainBefore(e, b, n), i.makeDOMNodeLine(e, b, n), h.getBeforeA(b, n, e), i.makeDOMNodeNameBefore(e, b, n), h.getInnerBeforeA(b, n, e), i.makeDOMNodeIcon(e, b, n), h.getInnerAfterA(b, n, e), i.makeDOMNodeNameAfter(e, b, n), h.getAfterA(b, n, e), n.isParent && n.open && i.makeUlHtml(b, n, e, m.join("")), i.makeDOMNodeMainAfter(e, b,
                n), h.addCreatedNode(b, n))
            } return e
        }, appendParentULDom: function (b, a) { var c = [], d = k("#" + a.tId), f = k("#" + a.tId + e.id.UL), g = i.appendNodes(b, a.level + 1, a[b.data.key.children], a, !1, !0); i.makeUlHtml(b, a, c, g.join("")); !d.get(0) && a.parentTId && (i.appendParentULDom(b, a.getParentNode()), d = k("#" + a.tId)); f.get(0) && f.remove(); d.append(c.join("")) }, asyncNode: function (b, a, c, d) {
            var f, g; if (a && !a.isParent) return j.apply(d), !1; else if (a && a.isAjaxing) return !1; else if (j.apply(b.callback.beforeAsync, [b.treeId, a], !0) == !1) return j.apply(d),
            !1; if (a) a.isAjaxing = !0, k("#" + a.tId + e.id.ICON).attr({ style: "", "class": e.className.BUTTON + " " + e.className.ICO_LOADING }); var l = {}; for (f = 0, g = b.async.autoParam.length; a && f < g; f++) { var q = b.async.autoParam[f].split("="), o = q; q.length > 1 && (o = q[1], q = q[0]); l[o] = a[q] } if (j.isArray(b.async.otherParam)) for (f = 0, g = b.async.otherParam.length; f < g; f += 2) l[b.async.otherParam[f]] = b.async.otherParam[f + 1]; else for (var m in b.async.otherParam) l[m] = b.async.otherParam[m]; var n = h.getRoot(b)._ver; k.ajax({
                contentType: b.async.contentType,
                type: b.async.type, url: j.apply(b.async.url, [b.treeId, a], b.async.url), data: l, dataType: b.async.dataType, success: function (f) { if (n == h.getRoot(b)._ver) { var g = []; try { g = !f || f.length == 0 ? [] : typeof f == "string" ? eval("(" + f + ")") : f } catch (l) { g = f } if (a) a.isAjaxing = null, a.zAsync = !0; i.setNodeLineIcos(b, a); g && g !== "" ? (g = j.apply(b.async.dataFilter, [b.treeId, a, g], g), i.addNodes(b, a, g ? j.clone(g) : [], !!c)) : i.addNodes(b, a, [], !!c); b.treeObj.trigger(e.event.ASYNC_SUCCESS, [b.treeId, a, f]); j.apply(d) } }, error: function (c, d, f) {
                    if (n ==
                    h.getRoot(b)._ver) { if (a) a.isAjaxing = null; i.setNodeLineIcos(b, a); b.treeObj.trigger(e.event.ASYNC_ERROR, [b.treeId, a, c, d, f]) }
                }
            }); return !0
        }, cancelPreSelectedNode: function (b, a) { for (var c = h.getRoot(b).curSelectedList, d = c.length - 1; d >= 0; d--) if (!a || a === c[d]) if (k("#" + c[d].tId + e.id.A).removeClass(e.node.CURSELECTED), a) { h.removeSelectedNode(b, a); break } if (!a) h.getRoot(b).curSelectedList = [] }, createNodeCallback: function (b) {
            if (b.callback.onNodeCreated || b.view.addDiyDom) for (var a = h.getRoot(b) ; a.createdNodes.length >
            0;) { var c = a.createdNodes.shift(); j.apply(b.view.addDiyDom, [b.treeId, c]); b.callback.onNodeCreated && b.treeObj.trigger(e.event.NODECREATED, [b.treeId, c]) }
        }, createNodes: function (b, a, c, d) { if (c && c.length != 0) { var f = h.getRoot(b), g = b.data.key.children, g = !d || d.open || !!k("#" + d[g][0].tId).get(0); f.createdNodes = []; a = i.appendNodes(b, a, c, d, !0, g); d ? (d = k("#" + d.tId + e.id.UL), d.get(0) && d.append(a.join(""))) : b.treeObj.append(a.join("")); i.createNodeCallback(b) } }, destroy: function (b) {
            b && (h.initCache(b), h.initRoot(b), m.unbindTree(b),
            m.unbindEvent(b), b.treeObj.empty())
        }, expandCollapseNode: function (b, a, c, d, f) {
            var g = h.getRoot(b), l = b.data.key.children; if (a) {
                if (g.expandTriggerFlag) { var q = f, f = function () { q && q(); a.open ? b.treeObj.trigger(e.event.EXPAND, [b.treeId, a]) : b.treeObj.trigger(e.event.COLLAPSE, [b.treeId, a]) }; g.expandTriggerFlag = !1 } if (!a.open && a.isParent && (!k("#" + a.tId + e.id.UL).get(0) || a[l] && a[l].length > 0 && !k("#" + a[l][0].tId).get(0))) i.appendParentULDom(b, a), i.createNodeCallback(b); if (a.open == c) j.apply(f, []); else {
                    var c = k("#" + a.tId +
                    e.id.UL), g = k("#" + a.tId + e.id.SWITCH), o = k("#" + a.tId + e.id.ICON); a.isParent ? (a.open = !a.open, a.iconOpen && a.iconClose && o.attr("style", i.makeNodeIcoStyle(b, a)), a.open ? (i.replaceSwitchClass(a, g, e.folder.OPEN), i.replaceIcoClass(a, o, e.folder.OPEN), d == !1 || b.view.expandSpeed == "" ? (c.show(), j.apply(f, [])) : a[l] && a[l].length > 0 ? c.slideDown(b.view.expandSpeed, f) : (c.show(), j.apply(f, []))) : (i.replaceSwitchClass(a, g, e.folder.CLOSE), i.replaceIcoClass(a, o, e.folder.CLOSE), d == !1 || b.view.expandSpeed == "" || !(a[l] && a[l].length >
                    0) ? (c.hide(), j.apply(f, [])) : c.slideUp(b.view.expandSpeed, f))) : j.apply(f, [])
                }
            } else j.apply(f, [])
        }, expandCollapseParentNode: function (b, a, c, d, f) { a && (a.parentTId ? (i.expandCollapseNode(b, a, c, d), a.parentTId && i.expandCollapseParentNode(b, a.getParentNode(), c, d, f)) : i.expandCollapseNode(b, a, c, d, f)) }, expandCollapseSonNode: function (b, a, c, d, f) {
            var g = h.getRoot(b), e = b.data.key.children, g = a ? a[e] : g[e], e = a ? !1 : d, j = h.getRoot(b).expandTriggerFlag; h.getRoot(b).expandTriggerFlag = !1; if (g) for (var k = 0, m = g.length; k < m; k++) g[k] &&
            i.expandCollapseSonNode(b, g[k], c, e); h.getRoot(b).expandTriggerFlag = j; i.expandCollapseNode(b, a, c, d, f)
        }, makeDOMNodeIcon: function (b, a, c) { var d = h.getNodeName(a, c), d = a.view.nameIsHTML ? d : d.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;"); b.push("<span id='", c.tId, e.id.ICON, "' title='' treeNode", e.id.ICON, " class='", i.makeNodeIcoClass(a, c), "' style='", i.makeNodeIcoStyle(a, c), "'></span><span id='", c.tId, e.id.SPAN, "'>", d, "</span>") }, makeDOMNodeLine: function (b, a, c) {
            b.push("<span id='", c.tId,
            e.id.SWITCH, "' title='' class='", i.makeNodeLineClass(a, c), "' treeNode", e.id.SWITCH, "></span>")
        }, makeDOMNodeMainAfter: function (b) { b.push("</li>") }, makeDOMNodeMainBefore: function (b, a, c) { b.push("<li id='", c.tId, "' class='", e.className.LEVEL, c.level, "' tabindex='0' hidefocus='true' treenode>") }, makeDOMNodeNameAfter: function (b) { b.push("</a>") }, makeDOMNodeNameBefore: function (b, a, c) {
            var d = h.getNodeTitle(a, c), f = i.makeNodeUrl(a, c), g = i.makeNodeFontCss(a, c), l = [], k; for (k in g) l.push(k, ":", g[k], ";"); b.push("<a id='",
            c.tId, e.id.A, "' class='", e.className.LEVEL, c.level, "' treeNode", e.id.A, ' onclick="', c.click || "", '" ', f != null && f.length > 0 ? "href='" + f + "'" : "", " target='", i.makeNodeTarget(c), "' style='", l.join(""), "'"); j.apply(a.view.showTitle, [a.treeId, c], a.view.showTitle) && d && b.push("title='", d.replace(/'/g, "&#39;").replace(/</g, "&lt;").replace(/>/g, "&gt;"), "'"); b.push(">")
        }, makeNodeFontCss: function (b, a) { var c = j.apply(b.view.fontCss, [b.treeId, a], b.view.fontCss); return c && typeof c != "function" ? c : {} }, makeNodeIcoClass: function (b,
        a) { var c = ["ico"]; a.isAjaxing || (c[0] = (a.iconSkin ? a.iconSkin + "_" : "") + c[0], a.isParent ? c.push(a.open ? e.folder.OPEN : e.folder.CLOSE) : c.push(e.folder.DOCU)); return e.className.BUTTON + " " + c.join("_") }, makeNodeIcoStyle: function (b, a) { var c = []; if (!a.isAjaxing) { var d = a.isParent && a.iconOpen && a.iconClose ? a.open ? a.iconOpen : a.iconClose : a.icon; d && c.push("background:url(", d, ") 0 0 no-repeat;"); (b.view.showIcon == !1 || !j.apply(b.view.showIcon, [b.treeId, a], !0)) && c.push("width:0px;height:0px;") } return c.join("") }, makeNodeLineClass: function (b,
        a) { var c = []; b.view.showLine ? a.level == 0 && a.isFirstNode && a.isLastNode ? c.push(e.line.ROOT) : a.level == 0 && a.isFirstNode ? c.push(e.line.ROOTS) : a.isLastNode ? c.push(e.line.BOTTOM) : c.push(e.line.CENTER) : c.push(e.line.NOLINE); a.isParent ? c.push(a.open ? e.folder.OPEN : e.folder.CLOSE) : c.push(e.folder.DOCU); return i.makeNodeLineClassEx(a) + c.join("_") }, makeNodeLineClassEx: function (b) { return e.className.BUTTON + " " + e.className.LEVEL + b.level + " " + e.className.SWITCH + " " }, makeNodeTarget: function (b) { return b.target || "_blank" },
        makeNodeUrl: function (b, a) { var c = b.data.key.url; return a[c] ? a[c] : null }, makeUlHtml: function (b, a, c, d) { c.push("<ul id='", a.tId, e.id.UL, "' class='", e.className.LEVEL, a.level, " ", i.makeUlLineClass(b, a), "' style='display:", a.open ? "block" : "none", "'>"); c.push(d); c.push("</ul>") }, makeUlLineClass: function (b, a) { return b.view.showLine && !a.isLastNode ? e.line.LINE : "" }, removeChildNodes: function (b, a) {
            if (a) {
                var c = b.data.key.children, d = a[c]; if (d) {
                    for (var f = 0, g = d.length; f < g; f++) h.removeNodeCache(b, d[f]); h.removeSelectedNode(b);
                    delete a[c]; b.data.keep.parent ? k("#" + a.tId + e.id.UL).empty() : (a.isParent = !1, a.open = !1, c = k("#" + a.tId + e.id.SWITCH), d = k("#" + a.tId + e.id.ICON), i.replaceSwitchClass(a, c, e.folder.DOCU), i.replaceIcoClass(a, d, e.folder.DOCU), k("#" + a.tId + e.id.UL).remove())
                }
            }
        }, setFirstNode: function (b, a) { var c = b.data.key.children; if (a[c].length > 0) a[c][0].isFirstNode = !0 }, setLastNode: function (b, a) { var c = b.data.key.children, d = a[c].length; if (d > 0) a[c][d - 1].isLastNode = !0 }, removeNode: function (b, a) {
            var c = h.getRoot(b), d = b.data.key.children,
            f = a.parentTId ? a.getParentNode() : c; a.isFirstNode = !1; a.isLastNode = !1; a.getPreNode = function () { return null }; a.getNextNode = function () { return null }; if (h.getNodeCache(b, a.tId)) {
                k("#" + a.tId).remove(); h.removeNodeCache(b, a); h.removeSelectedNode(b, a); for (var g = 0, l = f[d].length; g < l; g++) if (f[d][g].tId == a.tId) { f[d].splice(g, 1); break } i.setFirstNode(b, f); i.setLastNode(b, f); var j, g = f[d].length; if (!b.data.keep.parent && g == 0) f.isParent = !1, f.open = !1, g = k("#" + f.tId + e.id.UL), l = k("#" + f.tId + e.id.SWITCH), j = k("#" + f.tId + e.id.ICON),
                i.replaceSwitchClass(f, l, e.folder.DOCU), i.replaceIcoClass(f, j, e.folder.DOCU), g.css("display", "none"); else if (b.view.showLine && g > 0) { var o = f[d][g - 1], g = k("#" + o.tId + e.id.UL), l = k("#" + o.tId + e.id.SWITCH); j = k("#" + o.tId + e.id.ICON); f == c ? f[d].length == 1 ? i.replaceSwitchClass(o, l, e.line.ROOT) : (c = k("#" + f[d][0].tId + e.id.SWITCH), i.replaceSwitchClass(f[d][0], c, e.line.ROOTS), i.replaceSwitchClass(o, l, e.line.BOTTOM)) : i.replaceSwitchClass(o, l, e.line.BOTTOM); g.removeClass(e.line.LINE) }
            }
        }, replaceIcoClass: function (b, a, c) {
            if (a &&
            !b.isAjaxing && (b = a.attr("class"), b != void 0)) { b = b.split("_"); switch (c) { case e.folder.OPEN: case e.folder.CLOSE: case e.folder.DOCU: b[b.length - 1] = c } a.attr("class", b.join("_")) }
        }, replaceSwitchClass: function (b, a, c) {
            if (a) {
                var d = a.attr("class"); if (d != void 0) {
                    d = d.split("_"); switch (c) { case e.line.ROOT: case e.line.ROOTS: case e.line.CENTER: case e.line.BOTTOM: case e.line.NOLINE: d[0] = i.makeNodeLineClassEx(b) + c; break; case e.folder.OPEN: case e.folder.CLOSE: case e.folder.DOCU: d[1] = c } a.attr("class", d.join("_")); c !==
                    e.folder.DOCU ? a.removeAttr("disabled") : a.attr("disabled", "disabled")
                }
            }
        }, selectNode: function (b, a, c) { c || i.cancelPreSelectedNode(b); k("#" + a.tId + e.id.A).addClass(e.node.CURSELECTED); h.addSelectedNode(b, a) }, setNodeFontCss: function (b, a) { var c = k("#" + a.tId + e.id.A), d = i.makeNodeFontCss(b, a); d && c.css(d) }, setNodeLineIcos: function (b, a) {
            if (a) {
                var c = k("#" + a.tId + e.id.SWITCH), d = k("#" + a.tId + e.id.UL), f = k("#" + a.tId + e.id.ICON), g = i.makeUlLineClass(b, a); g.length == 0 ? d.removeClass(e.line.LINE) : d.addClass(g); c.attr("class",
                i.makeNodeLineClass(b, a)); a.isParent ? c.removeAttr("disabled") : c.attr("disabled", "disabled"); f.removeAttr("style"); f.attr("style", i.makeNodeIcoStyle(b, a)); f.attr("class", i.makeNodeIcoClass(b, a))
            }
        }, setNodeName: function (b, a) { var c = h.getNodeTitle(b, a), d = k("#" + a.tId + e.id.SPAN); d.empty(); b.view.nameIsHTML ? d.html(h.getNodeName(b, a)) : d.text(h.getNodeName(b, a)); j.apply(b.view.showTitle, [b.treeId, a], b.view.showTitle) && k("#" + a.tId + e.id.A).attr("title", !c ? "" : c) }, setNodeTarget: function (b) {
            k("#" + b.tId + e.id.A).attr("target",
            i.makeNodeTarget(b))
        }, setNodeUrl: function (b, a) { var c = k("#" + a.tId + e.id.A), d = i.makeNodeUrl(b, a); d == null || d.length == 0 ? c.removeAttr("href") : c.attr("href", d) }, switchNode: function (b, a) { a.open || !j.canAsync(b, a) ? i.expandCollapseNode(b, a, !a.open) : b.async.enable ? i.asyncNode(b, a) || i.expandCollapseNode(b, a, !a.open) : a && i.expandCollapseNode(b, a, !a.open) }
    }; k.fn.zTree = {
        consts: {
            className: { BUTTON: "button", LEVEL: "level", ICO_LOADING: "ico_loading", SWITCH: "switch" }, event: {
                NODECREATED: "ztree_nodeCreated", CLICK: "ztree_click",
                EXPAND: "ztree_expand", COLLAPSE: "ztree_collapse", ASYNC_SUCCESS: "ztree_async_success", ASYNC_ERROR: "ztree_async_error"
            }, id: { A: "_a", ICON: "_ico", SPAN: "_span", SWITCH: "_switch", UL: "_ul" }, line: { ROOT: "root", ROOTS: "roots", CENTER: "center", BOTTOM: "bottom", NOLINE: "noline", LINE: "line" }, folder: { OPEN: "open", CLOSE: "close", DOCU: "docu" }, node: { CURSELECTED: "curSelectedNode" }
        }, _z: { tools: j, view: i, event: m, data: h }, getZTreeObj: function (b) { return (b = h.getZTreeTools(b)) ? b : null }, destroy: function (b) {
            if (b && b.length > 0) i.destroy(h.getSetting(b));
            else for (var a in r) i.destroy(r[a])
        }, init: function (b, a, c) {
            var d = j.clone(L); k.extend(!0, d, a); d.treeId = b.attr("id"); d.treeObj = b; d.treeObj.empty(); r[d.treeId] = d; if (typeof document.body.style.maxHeight === "undefined") d.view.expandSpeed = ""; h.initRoot(d); b = h.getRoot(d); a = d.data.key.children; c = c ? j.clone(j.isArray(c) ? c : [c]) : []; b[a] = d.data.simpleData.enable ? h.transformTozTreeFormat(d, c) : c; h.initCache(d); m.unbindTree(d); m.bindTree(d); m.unbindEvent(d); m.bindEvent(d); c = {
                setting: d, addNodes: function (a, b, c) {
                    function e() {
                        i.addNodes(d,
                        a, h, c == !0)
                    } if (!b) return null; a || (a = null); if (a && !a.isParent && d.data.keep.leaf) return null; var h = j.clone(j.isArray(b) ? b : [b]); j.canAsync(d, a) ? i.asyncNode(d, a, c, e) : e(); return h
                }, cancelSelectedNode: function (a) { i.cancelPreSelectedNode(this.setting, a) }, destroy: function () { i.destroy(this.setting) }, expandAll: function (a) { a = !!a; i.expandCollapseSonNode(this.setting, null, a, !0); return a }, expandNode: function (a, b, c, e, m) {
                    if (!a || !a.isParent) return null; b !== !0 && b !== !1 && (b = !a.open); if ((m = !!m) && b && j.apply(d.callback.beforeExpand,
                    [d.treeId, a], !0) == !1) return null; else if (m && !b && j.apply(d.callback.beforeCollapse, [d.treeId, a], !0) == !1) return null; b && a.parentTId && i.expandCollapseParentNode(this.setting, a.getParentNode(), b, !1); if (b === a.open && !c) return null; h.getRoot(d).expandTriggerFlag = m; if (c) i.expandCollapseSonNode(this.setting, a, b, !0, function () { if (e !== !1) try { k("#" + a.tId).focus().blur() } catch (b) { } }); else if (a.open = !b, i.switchNode(this.setting, a), e !== !1) try { k("#" + a.tId).focus().blur() } catch (p) { } return b
                }, getNodes: function () { return h.getNodes(this.setting) },
                getNodeByParam: function (a, b, c) { return !a ? null : h.getNodeByParam(this.setting, c ? c[this.setting.data.key.children] : h.getNodes(this.setting), a, b) }, getNodeByTId: function (a) { return h.getNodeCache(this.setting, a) }, getNodesByParam: function (a, b, c) { return !a ? null : h.getNodesByParam(this.setting, c ? c[this.setting.data.key.children] : h.getNodes(this.setting), a, b) }, getNodesByParamFuzzy: function (a, b, c) { return !a ? null : h.getNodesByParamFuzzy(this.setting, c ? c[this.setting.data.key.children] : h.getNodes(this.setting), a, b) },
                getNodesByFilter: function (a, b, c, d) { b = !!b; return !a || typeof a != "function" ? b ? null : [] : h.getNodesByFilter(this.setting, c ? c[this.setting.data.key.children] : h.getNodes(this.setting), a, b, d) }, getNodeIndex: function (a) { if (!a) return null; for (var b = d.data.key.children, c = a.parentTId ? a.getParentNode() : h.getRoot(this.setting), e = 0, i = c[b].length; e < i; e++) if (c[b][e] == a) return e; return -1 }, getSelectedNodes: function () { for (var a = [], b = h.getRoot(this.setting).curSelectedList, c = 0, d = b.length; c < d; c++) a.push(b[c]); return a }, isSelectedNode: function (a) {
                    return h.isSelectedNode(this.setting,
                    a)
                }, reAsyncChildNodes: function (a, b, c) { if (this.setting.async.enable) { var j = !a; j && (a = h.getRoot(this.setting)); if (b == "refresh") { for (var b = this.setting.data.key.children, m = 0, p = a[b] ? a[b].length : 0; m < p; m++) h.removeNodeCache(d, a[b][m]); h.removeSelectedNode(d); a[b] = []; j ? this.setting.treeObj.empty() : k("#" + a.tId + e.id.UL).empty() } i.asyncNode(this.setting, j ? null : a, !!c) } }, refresh: function () {
                    this.setting.treeObj.empty(); var a = h.getRoot(this.setting), b = a[this.setting.data.key.children]; h.initRoot(this.setting); a[this.setting.data.key.children] =
                    b; h.initCache(this.setting); i.createNodes(this.setting, 0, a[this.setting.data.key.children])
                }, removeChildNodes: function (a) { if (!a) return null; var b = a[d.data.key.children]; i.removeChildNodes(d, a); return b ? b : null }, removeNode: function (a, b) { a && (b = !!b, b && j.apply(d.callback.beforeRemove, [d.treeId, a], !0) == !1 || (i.removeNode(d, a), b && this.setting.treeObj.trigger(e.event.REMOVE, [d.treeId, a]))) }, selectNode: function (a, b) {
                    if (a && j.uCanDo(this.setting)) {
                        b = d.view.selectedMulti && b; if (a.parentTId) i.expandCollapseParentNode(this.setting,
                        a.getParentNode(), !0, !1, function () { try { k("#" + a.tId).focus().blur() } catch (b) { } }); else try { k("#" + a.tId).focus().blur() } catch (c) { } i.selectNode(this.setting, a, b)
                    }
                }, transformTozTreeNodes: function (a) { return h.transformTozTreeFormat(this.setting, a) }, transformToArray: function (a) { return h.transformToArrayFormat(this.setting, a) }, updateNode: function (a) {
                    a && k("#" + a.tId).get(0) && j.uCanDo(this.setting) && (i.setNodeName(this.setting, a), i.setNodeTarget(a), i.setNodeUrl(this.setting, a), i.setNodeLineIcos(this.setting,
                    a), i.setNodeFontCss(this.setting, a))
                }
            }; b.treeTools = c; h.setZTreeTools(d, c); b[a] && b[a].length > 0 ? i.createNodes(d, 0, b[a]) : d.async.enable && d.async.url && d.async.url !== "" && i.asyncNode(d); return c
        }
    }; var M = k.fn.zTree, e = M.consts
})(jQuery);
