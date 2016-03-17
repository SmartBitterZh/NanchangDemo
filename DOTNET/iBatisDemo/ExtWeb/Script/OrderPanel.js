///<reference path="vswd-ext_2.2.js" />

iBatisDemo.Ext.OrderPanel = Ext.extend(Ext.Panel, {
    layout: "fit",
    closable: true,
    autoScroll: true,
    title: "订单列表",

    createForm:function() {
        var formPanel = new Ext.form.FormPanel({
            bodyStyle: "padding:15px 0 0 17px",
            baseCls: "header",
            labelWidth: 80,
            labelAlign: "right",
            items:[{
                xtype: "textfield",
                name:"OrderID",
                fieldLabel:"OrderID",
                disabled:true
            },{
                xtype:"textfield",
                name:"CustomID",
                fieldLabel:"CustomID",
                disabled:true
            },{
                xtype:"textfield",
                name:"Status",
                fieldLabel:"Status",
                disabled:true,
            },{
                xtype:"textfield",
                name:"CreateTime",
                fieldLabel:"CreateTime",
                disabled:true
            }]
        });
        return formPanel;
    },
    
    read: function() {
        var record = this.grid.getSelectionModel().getSelected();
        if(!record){
            Ext.Msg.alert("提示","请选择一个订单！");
            return;
        }
        
        this.showWin("查看订单详情");
        this.fp.form.loadRecord(record);
    },

    del: function() {
        var record = this.grid.getSelectionModel().getSelected();
        if(!record) {
            Ext.Msg.alert("提示","请选择一个订单！");
            return;
        }
        var orderId = record.get("OrderID");
        Ext.Msg.confirm("提示","确定要删除此订单吗？",function(btn){
            if(btn=="yes"){
                Ext.Ajax.request({
                    timeout:30000,
                    url:"Order.aspx?cmd=del",
                    params:{"orderId":orderId},
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
    
    closeWin:function() {
        if(this.win){
            this.win.close();
        }
        this.win = null;
        this.fp = null;
    },
    
    showWin:function(title){
        if(!this.win) {
            if(!this.fp){
                this.fp = this.createForm();
            }
            this.win = this.initWin(title);
            this.win.on("click",function(){
                this.win = null;
                this.fp = null;
            },this);
        }
        this.win.show();
    },
    
    initWin:function(title){
        var win= new Ext.Window({
            title:title,
            width:270,
            height:200,
            buttonAlign:"center",
            items:[this.fp],
            buttons:[{
                text:"取消",
                handler:this.closeWin,
                scope:this
            }]
        });
        return win;
    },

    initComponent: function() {
        this.store = new Ext.data.JsonStore({
            autoDestory: true,
            url: "Order.aspx?cmd=list",
            root: "result",
            idProperty: "OrderID",
            fields: ["OrderID", "CustomID", "Status", "CreateTime"]
        });

        this.grid = new Ext.grid.GridPanel({
            store: this.store,
            baseCls: "my-panel-no-border",   //自定义CSS来设置GridPanel的边框为无
            tbar: [{
                text: "查看",
                handler: this.read,
                scope: this
            }, new Ext.Toolbar.Separator(), {
                text: "删除",
                handler: this.del,
                scope: this
}],
columns: [{
    header:"OrderID",
    width:80,
    dataIndex:"OrderID"
                },{
                    header:"CustomID",
                    width:80,
                    dataIndex:"CustomID"
                },{
                    header:"Status",
                    width:80,
                    dataIndex:"Status"
                },{
                    header:"CreateTime",
                    width:120,
                    dataIndex:"CreateTime"
                }]
            });

            iBatisDemo.Ext.OrderPanel.superclass.initComponent.call(this);
            this.add(this.grid);
            this.store.reload();
        }
    });