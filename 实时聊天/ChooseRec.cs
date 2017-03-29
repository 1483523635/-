using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 实时聊天
{
    public partial class ChooseRec : Form
    {
        #region 字段的声明
        /// <summary>
        /// 用于存放添加的items
        /// 用HashSet可以避免重复
        /// </summary>
        HashSet<string> List_add = new HashSet<string>();
        /// <summary>
        /// 用于存放移除的Items
        /// </summary>
        HashSet<string> List_remove = new HashSet<string>();

        private IList<TcpClient> clientList_file;
        #endregion

        public ChooseRec(IList<TcpClient> clientList_file) 
        {
            this.clientList_file = clientList_file;
            InitializeComponent();
            CbList_Online.Items.Clear();
            Set_CbList_OnlineDate<TcpClient>(clientList_file);

        }
        #region 设置list的默认数据


        private void Set_CbList_OnlineDate<T>(IList<T> list)
        {
            foreach (T item in list)
            {
                CbList_Online.Items.Add((item as TcpClient).Client.RemoteEndPoint.ToString());
            }
           
        }
        #endregion

        #region 选中事件处理

        #region 在线用户


        /// <summary>
        /// 当checkedListBox的对号选中时的事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CbList_Online_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if(e.NewValue==CheckState.Checked)
            {
                List_add.Add((sender as CheckedListBox).SelectedItem.ToString());
            }          
        }
        #endregion

        #region 接收用户

       
        private void Cbl_rec_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.NewValue == CheckState.Checked)
            {
                List_remove.Add((sender as CheckedListBox).SelectedItem.ToString());
            }
        }
        #endregion

        #endregion

        #region button事件处理  
        #region 添加选中的


        private void btn_Add_Click(object sender, EventArgs e)
        {
            foreach (string item in List_add)
            {
                Cbl_rec.Items.Add(item);
                CbList_Online.Items.Remove(item);
            }
            List_add.Clear();
        }
        #endregion

        #region 移除选中的  
        private void btn_Remove_Click(object sender, EventArgs e)
        {
            foreach (string item in List_remove)
            {
                Cbl_rec.Items.Remove(item);
                CbList_Online.Items.Add(item);
            }
            List_remove.Clear();
        }
        #endregion

        #region 添加所有  
        private void btn_addAll_Click(object sender, EventArgs e)
        {
            foreach (var item in CbList_Online.Items)
            {
                Cbl_rec.Items.Add(item);
            }
            CbList_Online.Items.Clear();
        }
        #endregion

        #region 移除所有   
        private void btn_removeAll_Click(object sender, EventArgs e)
        {
            foreach (var item in Cbl_rec.Items)
            {
                CbList_Online.Items.Add(item);
            }
            Cbl_rec.Items.Clear();
        }
        #endregion
        #endregion


        #region 获取所有接受者 
        public List<TcpClient> GetAllRecvier()
        {
            List<TcpClient> users = new List<TcpClient>();
           
            foreach (var item in Cbl_rec.Items)
            {
                foreach (var item1 in clientList_file)
                {
                    if (string.Equals(item,(item1 as TcpClient).Client.RemoteEndPoint.ToString()))
                    users.Add(item1);
                }
               
            }
            return users;
        }
        #endregion

        private void btn_ok_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
