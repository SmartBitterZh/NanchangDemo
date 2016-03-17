Ext.onReady(function() {
var mainPanel = new Ext.TabPanel({
    region: "center",
        id: "mainPanel",
});

var rootNode = new Ext.tree.TreeNode({
    rootVisible: false
});

rootNode.appendChild(new Ext.tree.TreeNode({
    id: "nodeMenu1",
    text: "产品列表",
    listeners: {
        "click": function() {
            var newProductsPanel = Ext.getCmp("productsPanel");
            if (!newProductsPanel) {
                newProductsPanel = new iBatisDemo.Ext.ProductsPanel({ region:"center", id: "productsPanel" });
            }
            mainPanel.add(newProductsPanel);
            mainPanel.setActiveTab(newProductsPanel);
        }
    }
}));
rootNode.appendChild(new Ext.tree.TreeNode({
    id: "nodeMenu2",
    text: "订单列表",
    listeners: {
        "click": function() {
            var orderPanel= Ext.getCmp("orderPanel");
            if(!orderPanel){
                orderPanel = new iBatisDemo.Ext.OrderPanel({region:"center", id:"orderPanel"});
            }
            mainPanel.add(orderPanel);
            mainPanel.setActiveTab(orderPanel);
        }
    }
}));
rootNode.appendChild(new Ext.tree.TreeNode({
    id: "nodeMenu3",
    text: "购物车",
    listeners: {
        "click": function() {
            var shopCarPanel = Ext.getCmp("shopCarPanel");
            if(!shopCarPanel){
                shopCarPanel = new iBatisDemo.Ext.ShopCarPanel({region:"center", id:"shopCarPanel"});
            }
            mainPanel.add(shopCarPanel);
            mainPanel.setActiveTab(shopCarPanel);
        }
    }
}));



new Ext.Viewport({
    layout: "border",
    items: [{
        region: "north",
        height: 36,
        baseCls: "header",
        html: "<div style='padding:10px 0 0 15px; font-weight:bolder;'>系统管理后台</div>"
    }, {
        region: "west",
        collapsible: true,
        title: '栏目导航',
        xtype: 'treepanel',
        width: 150,
        autoScroll: true,
        split: true,
        //loader: new Ext.tree.TreeLoader(),
        root: rootNode,
        rootVisible: false
    }, mainPanel]
    });
});