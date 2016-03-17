Ext.namespace("iBatisDemo.Ext");

//遗留的问题Panel的Border不能隐藏，太粗了
iBatisDemo.Ext.ProductsPanel = Ext.extend(Ext.Panel, {
    //layout: "border", //千万不能加此句，会报错
    layout:"fit",
    closable: true,
  	autoScroll:true,
    title: '产品列表',
    
    createForm:function() {
        var formPanel = new Ext.form.FormPanel({
            bodyStyle: "padding:15px 0 0 27px",
            baseCls: "header",
            labelWidth:70,
            lableAlign:"right",
            items:[{
                    xtype:"hidden",
                    name:"ID"
                },
                {
                    xtype:"textfield",
                    fieldLabel: "ProductName",
                    name: "ProductName",
                    allowBlank: false,
                },{xtype:"numberfield",
                    fieldLabel: "NormalPrice",
                    name: "NormalPrice",
                    allowBlank: false,
                },{xtype:"numberfield",
                    fieldLabel: "MemberPrice",
                    name: "MemberPrice",
                    allowBlank: false,
            }]
        });
        return formPanel;
    },
    
    create:function(){
        this.showWin("添加产品");
    },
    
    edit:function() {
        var record = this.grid.getSelectionModel().getSelected();
        if(!record) {
            Ext.Msg.alert("提示", "请选择一行编辑数据！");
            return;
        }
        var id = record.get("id");
        this.showWin("编辑产品");
        this.fp.form.loadRecord(record);
    },
    
    save:function(){
        var id = this.fp.form.findField("ID").getValue();
        this.fp.form.submit({
            waitMsg:"正在保存......",
            url:"Product.aspx?cmd=save&productid="+id,
            success:function(){
                this.closeWin();
                this.store.reload();
            },
            scope:this
        });
    },
    del:function(){
        var record = this.grid.getSelectionModel().getSelected();
        if(!record) {
            Ext.Msg.alert("提示", "请选择一行删除数据！");
            return;
        }
        var productId = record.get("ID");
        Ext.Msg.confirm("提示" , "确定要删除该数据吗？", function(btn){
            if(btn=="yes") {
                Ext.Ajax.request({
                   timeout:30000,
                   url: 'Product.aspx?cmd=del',
                   params: { "productid": productId },
                   method:"GET",
                   success: function(res){
                        var r = Ext.decode(res.responseText);
                        if(!r.success) {
                            Ext.Msg.alert("提示信息","删除数据失败！");
                        }else{
                            Ext.Msg.alert("提示信息","删除数据成功！");
                            this.store.reload();
                        }
                   },
                   scope:this
                });
            }
        } , this);
    },
    
    addShopCar:function(){
        var record = this.grid.getSelectionModel().getSelected();
        if(!record) {
            Ext.Msg.alert("提示","您还没有选择产品！");
            return;
        }
        var productId = record.get("ID");
        Ext.Ajax.request({
            timeout:30000,
            url:"ShopCar.aspx?cmd=add&productid="+productId,
            success:function(res){
                var r = Ext.decode(res.responseText);
                if(r.success) {
                    Ext.Msg.alert("提示","已经将产品放入了购物车！");
                }else{
                    Ext.Msg.alert("提示","产品放入购物车失败！");
                }
            },
            scope:this
        });
    },
    
    reset:function(){
        if(this.win){
            this.fp.form.reset();
        }
    },
    
    closeWin:function(){
        if(this.win) {
            this.win.close();
        }
        this.win=null;
        this.fp=null;
    },
    
    initWin:function(title) {
        var win = new Ext.Window({
            title:title,
            width:270,
            height:170,
            closeAction:"hide",
            buttonAlign:"center",
            items:[this.fp],
            buttons:[{ 
                text:"保存",
                handler:this.save,
                scope:this
            },{
                text:"清空",
                handler:this.reset,
                scope:this
            },{
                text:"取消",
                handler:this.closeWin,
                scope:this
            }]
        });
        return win;
    },
    
    showWin:function(title){
        if(!this.win) {
            if(!this.fp){
                this.fp = this.createForm();
            }
            this.win = this.initWin(title);
            this.win.on("close",function(){
                    this.win=null;
                    this.fp=null;
                },this);
        }
        this.win.show();
    },

    initComponent: function() {
        this.store = new Ext.data.JsonStore({
            autoDestroy: true,
            url: "Product.aspx?cmd=list",
            root: "result",
            idProperty: "ID",
            fields: ["ID", "ProductName", "NormalPrice", "MemberPrice"]
        });

        this.grid = new Ext.grid.GridPanel({
            store: this.store,
            baseCls:"my-panel-no-border",   //自定义CSS来设置GridPanel的边框为无
            tbar: [{
                    text: '添加',
                    handler:this.create,
                    scope: this
            },new Ext.Toolbar.Separator({}),
            {
                    text: '修改',
                    handler:this.edit,
                    scope: this
            },new Ext.Toolbar.Separator({}),
            {
                    text: '删除',
                    handler:this.del,
                    scope: this
            },new Ext.Toolbar.Separator({}),
            {
                    text:"加入购物车",
                    handler:this.addShopCar,
                    scope:this
            }],
            columns: [{
                    header: "ID",
                    dataIndex: "ID",
                    width: 80
                }, {
                    header: "ProductName",
                    dataIndex: "ProductName",
                    width: 200
                }, {
                    header: "NormalPrice",
                    dataIndex: "NormalPrice",
                    width: 120
                }, {
                    header: "MemberPrice",
                    dataIndex: "MemberPrice",
                    width: 120
            }]
        });

        iBatisDemo.Ext.ProductsPanel.superclass.initComponent.call(this);
        this.add(this.grid);
        this.store.load();
    }
});