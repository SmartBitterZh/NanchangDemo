///<reference path="vswd-ext_2.2.js" />

iBatisDemo.Ext.ShopCarPanel = Ext.extend(Ext.Panel, {
    layout: "fit",
    closable: true,
    autoScoll: true,
    title: "我的购物车",

    createForm: function() {
        var formPanel = new Ext.form.FormPanel({
            bodyStyle: "padding:15px 0 0 17px",
            baseCls: "header",
            labelWidth: 80,
            labelAlign: "right",
            items: [{
                xtype: "hidden",
                name: "ProductID"
            }, {
                xtype: "textfield",
                name: "ProductName",
                disabled:true,
                fieldLabel: "ProductName"
            }, {
                xtype: "textfield",
                disabled:true,
                name: "Price",
                fieldLabel: "Price"
            }, {
                xtype: "numberfield",
                name: "Num",
                allowBlank: false,
                fieldLabel: "Num"
}]
            });
            return formPanel;
        },
        
        edit:function(){
            var record = this.grid.getSelectionModel().getSelected();
            if(!record){
                Ext.Msg.alert("提示","请选择一个产品！");
                return;
            }
            this.showWin("编辑产品数量");
            this.fp.form.loadRecord(record);
        },
        
        closeWin:function(){
            if(this.win){
                this.win.close();
            }
            this.win = null;
            this.fp = null;
        },
        
        save:function(){
            var productId= this.fp.form.findField("ProductID").getValue();
            this.fp.form.submit({
                waitMsg:"正在保存......",
                url:"ShopCar.aspx?cmd=edit&productId="+productId,
                success:function(){
                    this.win.close();
                    this.store.reload();
                },
                scope:this
            
            });
        },
        
        del:function(){
            var record = this.grid.getSelectionModel().getSelected();
            if(!record){
                Ext.Msg.alert("提示","请选择一个产品！");
                return;
            }
            var productId = record.get("ProductID");
            Ext.Msg.confirm("提示","确定要删除此产品吗？", function(btn){
                if(btn=="yes"){
                    Ext.Ajax.request({
                        timeout:30000,
                        url:"ShopCar.aspx?cmd=del",
                        params:{"productId": productId},
                        method:"GET",
                        success:function(res){
                            var r = Ext.decode(res.responseText);
                            if(r.success){
                                Ext.Msg.alert("提示","删除成功！");
                                this.store.reload();
                            }else{
                                Ext.Msg.alert("提示","删除失败！");
                            }
                        },
                        scope:this
                    });
                }
            },this);
        },
        
        ordered:function(){
            Ext.Ajax.request({
                        timeout:30000,
                        url:"ShopCar.aspx?cmd=order",
                        method:"GET",
                        success:function(res){
                            var r = Ext.decode(res.responseText);
                            if(r.success){
                                Ext.Msg.alert("提示","下订成功！");
                                this.store.reload();
                            }else{
                                Ext.Msg.alert("提示","下订失败！");
                            }
                        },
                        scope:this
                    });
        },

        initWin: function(title) {
            var win = new Ext.Window({
                title: title,
                width:270,
                height:170,
                buttonAlign:"center",
                items:[this.fp],
                buttons:[{
                    text:"保存",
                    handler:this.save,
                    scope:this,
                    },{
                    text:"取消",
                    handler:this.closeWin,
                    scope:this
                }]
            });
            return win;
        },
        
        showWin:function(title){
            if(!this.win){
                if(!this.fp){
                    this.fp = this.createForm();
                }
                this.win = this.initWin(title);
                this.win.on("close",function(){
                    this.win = null;
                    this.fp= null;
                },this);
            }
            this.win.show();
        },

        initComponent: function() {
            this.store = new Ext.data.JsonStore({
                autoDestory: true,
                url: "ShopCar.aspx?cmd=list",
                root: "result",
                idPropery: "ProductID",
                fields: ["NO", "ProductID", "ProductName", "Price", "Num"]
            });

            this.grid = new Ext.grid.GridPanel({
                store: this.store,
                baseCls: "my-panel-no-border",   //自定义CSS来设置GridPanel的边框为无
                tbar: [{
                    text: "修改",
                    handler:this.edit,
                    scope: this
                }, new Ext.Toolbar.Separator(),
            {
                text: "删除",
                handler:this.del,
                scope: this
}, new Ext.Toolbar.Separator(),{
    text:"订单",
    handler:this.ordered,
    scope:this
}],
                columns: [{
                    header: "NO",
                    width: 80
                }, {
                    header: "ProductID",
                    hidden: true,
                    dataIndex: "ProductID"
                }, {
                    header: "ProductName",
                    dataIndex: "ProductName",
                    width: 160
                }, {
                    header: "Price",
                    dataIndex: "Price",
                    width: 100
                }, {
                    header: "Num",
                    dataIndex: "Num"
}]
                });

                iBatisDemo.Ext.ShopCarPanel.superclass.initComponent.call(this);
                this.add(this.grid);
                this.store.reload();
            }
        });