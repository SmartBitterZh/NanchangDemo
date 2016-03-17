using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPFWithOracle.Helper;
using WPFWithOracle.Model.Discount;
using WPFWithOracle.MySocketServer;
using WPFWithOracle.Presention;
using WPFWithOracle.Presention.Products;
using WPFWithOracle.Service.Products;

namespace WPFWithOracle.WPFUI
{
  /// <summary>
  /// MainWindow.xaml 的交互逻辑
  /// </summary>
  public partial class MainWindow : Window, IProductListView
  {
    private ProductListPresenter _presenter;

    public MainWindow()
    {
      InitializeComponent();
      init();
      _presenter.Display();
    }

    protected void init()
    {
      this.ddlCustType.ItemsSource = Enum.GetValues(typeof(CustomerType));
      this.ddlCustType.SelectedIndex = 0;
      this.ddlCustType.SelectionChanged += delegate { _presenter.Display(); };
      _presenter = new ProductListPresenter(this, ObjectFactory.GetInstance<ProductService>());
    }

    public CustomerType CustomerType
    {
      get { return (CustomerType)Enum.ToObject(typeof(CustomerType), this.ddlCustType.SelectedIndex); }
    }

    public string ErrorMessage
    {
      set { txtError.Text = String.Format("<p><strong>Error</strong><br/>{0}<p/>", value); }
    }

    public void Display(IList<ProductViewModel> Products)
    {
      gridProducts.ItemsSource = Products;
    }
    SocketClient _client;
    private void btnConnectServer_Click(object sender, RoutedEventArgs e)
    {
      string _ip = this.txtIP.Text;
      string _port = this.txtPort.Text;
      _client = new SocketClient(_ip, int.Parse(_port));
      _client.OnSended += _client_OnSended;
      _client.OnMsgReceived += _client_OnMsgReceived;
    }

    void _client_OnMsgReceived(string info)
    {
      this.txtError.Text = info;
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
