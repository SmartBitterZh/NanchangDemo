

Ext.onReady(function() {
    // 开启表单提示
    Ext.QuickTips.init();

    // 创建一个FormPanel
    var formPanel = new Ext.form.FormPanel({
        id: "loginForm",
        labelWidth: 50, // 默认标签宽度板 
        labelAlign: "right",
        bodyStyle: "padding:5px 5px 0",
        buttonAlign: "center",
        // 不设置该值，表单将保持原样，设置后表单与窗体完全融合 
        baseCls: "header",

        defaults: {
            width: 200
        },
        // 默认字段类型 
        defaultType: "textfield",
        items: [{
            fieldLabel: "用户名",
            name: "CustomID",
            allowBlank: false,
            value: "1"
        },
            {
                fieldLabel: "密　码",
                name: "password",
                inputType: "password",
                allowBlank: false,
                value: "1"}],
        buttons: [{
            text: "登录",
            type: "submit",
            handler: function() {
                Ext.getCmp("loginForm").getForm().submit({
                    waitMsg: '正在登录......',
                    clientValidation: true,
                    url: "Enter.aspx?cmd=login",
                    //params: {cmd: "login"},
                    success: function(form, action) {
                        window.location = "main.aspx";
                    },
                    failure: function(form, action) {
                        switch (action.failureType) {
                            case Ext.form.Action.CLIENT_INVALID:
                                Ext.Msg.alert("登录失败", "有一些必填项未填写！");
                                break;
                            case Ext.form.Action.CONNECT_FAILURE:
                                Ext.Msg.alert("登录失败", "Ajax 通迅失败！");
                                break;
                            case Ext.form.Action.SERVER_INVALID:
                                Ext.Msg.alert("登录失败", action.result.msg);
                                break;
                        }
                    }
                });
            }
        }, { text: "重置",
            type: "reset",
            handler: function() {
                Ext.getCmp("loginForm").getForm().reset();
            } }]
        });

        // 创建一个登录窗口
        var loginWin = new Ext.Window({
            title: "欢迎登录",
            width: 290,
            height: 126,
            plain: true,
            items: [formPanel]
        });

        loginWin.show();
    });