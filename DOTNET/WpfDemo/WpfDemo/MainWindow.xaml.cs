using DBControl;
using MySocketServer;
using MySocketServer.SocketHelper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDemo
{
  /// <summary>
  /// MainWindow.xaml 的交互逻辑
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      InitializeComponent();
    }
    public void InsertText(TextBlock control, string msg)
    {
      control.Dispatcher.Invoke(new Action(() => { control.Text += msg + "\n"; }));
    }
    public void BindGrid(DataGrid control, IList<Product> list)
    {
      control.Dispatcher.Invoke(new Action(() => { control.ItemsSource = list; }));
    }

    SocketClient _client;
    bool _connect = false;
    private void btnConnectServer_Click(object sender, RoutedEventArgs e)
    {
      if (_client == null || !_connect)
      {
        string _ip = this.txtIP.Text;
        string _port = this.txtPort.Text;
        _client = new SocketClient(_ip, int.Parse(_port));
        _client.OnSended += _client_OnSended;
        _client.OnMsgReceived += _client_OnMsgReceived;

        if (_connect = _client.Connect())
        {
          InsertText(txtMSGList, "Connect successfull.");
          _client.SendMessage("Hello Server.");
        }
        else
          InsertText(txtMSGList, "failed");
      }
    }

    private void btnSend_Click(object sender, RoutedEventArgs e)
    {
      if (this.txtMSG.Text == "FindAll_Product")
        _client.SendRequest(new BaseRequest() { UID = "Server", Message = "FindAll_Product", MessageCommand = MessageFunction.FindAll, MessageType = MessageEntityType.JSON });

      else
        _client.SendMessage(this.txtMSG.Text);
    }

    void _client_OnMsgReceived(string info)
    {
      InsertText(txtMSGList, info);

      string[] _msgs = RequestHandler.GetActualString(info);
      if (_msgs.Length > 0)
      {
        BaseResponse<Product> _response = SerializeHelper.JsonDeserialize<BaseResponse<Product>>(_msgs[0]);
        if (_response.Context != null)
          BindGrid(this.gridProducts, _response.Context as IList<Product>);
      }
    }

    void _client_OnSended(bool successorfalse)
    {
    }

    private void btnDelete_Click(object sender, RoutedEventArgs e)
    {
      this.gridProducts.Items.RemoveAt(this.gridProducts.SelectedIndex);
    }

    private void btnUpdate_Click(object sender, RoutedEventArgs e)
    {

    }
  }
}
