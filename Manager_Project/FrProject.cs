using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;
using System.IO;
using Sunny.UI;

namespace VDF3_Solution3
{
    public partial class FrProject : Form
    {
        private List<RecentProject> recentProjects = new List<RecentProject>();
        private ContextMenuStrip contextMenu;
        private string filePath1 = null; // Đường dẫn thư mục lưu file
        public static CalibrationService calibService = new CalibrationService();
        private string jsonContent;
        private string Curentprojectpath;

        public static FrProject Instance { get; private set; }

        public FrProject()
        {
            InitializeComponent();
            Instance = this;
            this.Text = string.Empty;
            this.ControlBox = false;
            this.DoubleBuffered = true;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeListView();
            InitializeContextMenu();
        }

        private void InitializeListView()
        {
            // Thêm các nhóm
            listView1.Groups.Add(new ListViewGroup("Pinned", HorizontalAlignment.Left) { Name = "Pinned" });
            listView1.Groups.Add(new ListViewGroup("Today", HorizontalAlignment.Left) { Name = "Today" });
            listView1.Groups.Add(new ListViewGroup("Yesterday", HorizontalAlignment.Left) { Name = "Yesterday" });
            listView1.Groups.Add(new ListViewGroup("This Month", HorizontalAlignment.Left) { Name = "This Month" });

            listView1.Columns.Add("Name", 130);
            listView1.Columns.Add("Path", 340);
            listView1.Columns.Add("Last Opened", 120);

            listView1.FullRowSelect = true;
            listView1.View = View.Details;
        }

        void SortPinnedGroupFirst()
        {
            var pinnedGroup = listView1.Groups["Pinned"];
            if (pinnedGroup != null)
            {
                listView1.Groups.Remove(pinnedGroup);
                listView1.Groups.Insert(0, pinnedGroup); // Đưa nhóm "Pinned" lên đầu
            }
        }

        private void InitializeContextMenu()
        {
            contextMenu = new ContextMenuStrip();

            var pinItem = new ToolStripMenuItem("Pin");
            var copyPathItem = new ToolStripMenuItem("Copy Path");
            var removeItem = new ToolStripMenuItem("Remove Item");

            contextMenu.Items.Add(removeItem);
            contextMenu.Items.Add(pinItem);
            contextMenu.Items.Add(copyPathItem);

            listView1.ContextMenuStrip = contextMenu;

            // Xử lý sự kiện
            listView1.MouseClick += (sender, e) =>
            {
                if (e.Button == MouseButtons.Right && listView1.FocusedItem != null)
                {
                    var selectedItem = listView1.FocusedItem;
                    var project = (RecentProject)selectedItem.Tag;

                    // Cập nhật text "Pin/Unpin" trong menu
                    pinItem.Text = project.IsPinned ? "Unpin" : "Pin";
                }
            };

            pinItem.Click += (sender, e) => TogglePinStatus();
            copyPathItem.Click += (sender, e) => CopyPath();
            removeItem.Click += (sender, e) => RemoveItem();
        }

        private void TogglePinStatus()
        {
            if (listView1.FocusedItem != null)
            {
                var selectedItem = listView1.FocusedItem;
                var project = (RecentProject)selectedItem.Tag;

                // Chuyển trạng thái
                project.IsPinned = !project.IsPinned;

                // Cập nhật nhóm và giao diện
                if (project.IsPinned)
                {
                    selectedItem.Group = listView1.Groups["Pinned"];
                }
                else
                {
                    if (project.LastOpened.Date == DateTime.Now.Date)
                        selectedItem.Group = listView1.Groups["Today"];
                    else if (project.LastOpened.Date == DateTime.Now.AddDays(-1).Date)
                        selectedItem.Group = listView1.Groups["Yesterday"];
                    else
                        selectedItem.Group = listView1.Groups["This Month"];
                }

                selectedItem.ForeColor = project.IsPinned ? Color.Blue : Color.Black;
                LoadProjectsIntoListView(); // Tải lại danh sách
            }
        }

        private void RemoveItem()
        {
            if (listView1.FocusedItem != null)
            {
                var selectedItem = listView1.FocusedItem;
                var project = (RecentProject)selectedItem.Tag;

                // Xóa khỏi danh sách RecentProject
                recentProjects.Remove(project);

                // Xóa khỏi ListView
                listView1.Items.Remove(selectedItem);
            }
        }


        private void CopyPath()
        {
            if (listView1.FocusedItem != null)
            {
                var selectedItem = listView1.FocusedItem;
                var project = (RecentProject)selectedItem.Tag;

                Clipboard.SetText(project.Path); // Sao chép vào clipboard
            }
        }

        private void LoadProjectsIntoListView()
        {
            listView1.Items.Clear();

            foreach (var project in recentProjects)
            {
                var item = new System.Windows.Forms.ListViewItem(project.Name);
                item.SubItems.Add(project.Path);
                item.SubItems.Add(project.LastOpened.ToString("g"));
                item.Tag = project;
                if (project.IsPinned)
                {
                    item.Group = listView1.Groups["Pinned"];
                    item.ForeColor = Color.Blue;
                }
                else
                {
                    if (project.LastOpened.Date == DateTime.Now.Date)
                        item.Group = listView1.Groups["Today"];
                    else if (project.LastOpened.Date == DateTime.Now.AddDays(-1).Date)
                        item.Group = listView1.Groups["Yesterday"];
                    else
                        item.Group = listView1.Groups["This Month"];
                }

                listView1.Items.Add(item);
            }

            SortPinnedGroupFirst();
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && listView1.FocusedItem != null)
            {
                contextMenuStrip1.Show(Cursor.Position);
            }
        }

        private void removeFromListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.FocusedItem != null)
            {
                listView1.Items.Remove(listView1.FocusedItem);
            }
        }

        private void pinToRecentListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.FocusedItem != null)
            {
                var item = listView1.FocusedItem;
                item.Group = null; // Chuyển về nhóm "Pinned"
            }
        }

        private void copyPathToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.FocusedItem != null)
            {
                var path = listView1.FocusedItem.SubItems[1].Text;
                Clipboard.SetText(path);
            }
        }

        private static readonly string filePath = Path.Combine(Application.StartupPath, "recent_projects.json");
        public static void SaveRecentProjects(List<RecentProject> projects)
        {
            var json = JsonConvert.SerializeObject(projects, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }

        public static List<RecentProject> LoadRecentProjects()
        {
            if (!File.Exists(filePath))
                return new List<RecentProject>();

            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<RecentProject>>(json);
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            // Kiểm tra đầu vào
            if (string.IsNullOrEmpty(filePath1))
            {
                MessageBox.Show("Please select a folder to save the project!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(txtProjectName.Text.Trim()))
            {
                MessageBox.Show("Project name cannot be empty!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Xây dựng đường dẫn file
            string destinationFile = Path.Combine(filePath1, $"{txtProjectName.Text.Trim()}.json");

            // Gọi hàm xử lý lưu file
            //ConfigManager.CoppyAndSaveAs("config.json", destinationFile, 0);

            // Hiển thị thông báo thành công
            MessageBox.Show("Project created successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            SystemCongfig.PresentFilePath = destinationFile;

            recentProjects.Add(new RecentProject(txtProjectName.Text, txtProjectPath.Text + "\\" + txtProjectName.Text + ".json", DateTime.Now, false));
            SaveRecentProjects(recentProjects);
            LoadProjectsIntoListView();
        }

        private void btnBrower_Click(object sender, EventArgs e)
        {
            // Tạo FolderBrowserDialog để chọn thư mục
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                folderBrowserDialog.Description = "Select folder to save the project file";
                folderBrowserDialog.ShowNewFolderButton = true;
                folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    filePath1 = folderBrowserDialog.SelectedPath;
                    txtProjectPath.Text = filePath1;
                }
            }
        }

        public void SaveProject()
        {
            if (SystemCongfig.PresentFilePath != null)
            {
                File.WriteAllText(SystemCongfig.PresentFilePath, calibService.SaveToJson());
            }
            else
                MessageBox.Show("Can't find the Project file path", "Path not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            if (listView1.FocusedItem != null)
            {
                var selectedItem = listView1.FocusedItem;
                var project = (RecentProject)selectedItem.Tag;

                // Kiểm tra xem đường dẫn có tồn tại không
                if (!System.IO.File.Exists(project.Path) && !System.IO.Directory.Exists(project.Path))
                {
                    // Hiển thị cảnh báo
                    MessageBox.Show($"The path \"{project.Path}\" does not exist. The item will be removed from the list.", "Path Not Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // Xóa mục khỏi danh sách RecentProjects
                    recentProjects.Remove(project);

                    // Xóa mục khỏi ListView
                    listView1.Items.Remove(selectedItem);

                    SaveRecentProjects(recentProjects);

                    return; // Thoát khỏi hàm
                }

                Curentprojectpath = project.Path;

                jsonContent = File.ReadAllText(project.Path);
                calibService.LoadFromJson(jsonContent);
                List<CalibrationPoint> currentPoints = calibService.GetCalibrationPoints();
                for (int i = 0; i < currentPoints.Count; i++)
                {
                    VariableRobot.calibrationArray[0, i] = currentPoints[i].x; // Hàng 0: tọa độ x
                    VariableRobot.calibrationArray[1, i] = currentPoints[i].y; // Hàng 1: tọa độ y
                }
                Register currentRegister = calibService.GetRegister();

                HomographyMatrix currentHM = calibService.GetHomographyMatrix();
                VariableRobot.R11 = currentHM.R11;
                VariableRobot.R12 = currentHM.R12;
                VariableRobot.Tx = currentHM.Tx;
                VariableRobot.R21 = currentHM.R21;
                VariableRobot.R22 = currentHM.R22;
                VariableRobot.Ty = currentHM.Ty;

                System.Drawing.Image loadedImage = calibService.GetTemplateImage();
                if (loadedImage != null)
                {
                    SystemMode.ImgTemplate = loadedImage;
                }

                SystemCongfig.PresentFilePath = project.Path;

                // Cập nhật lại thời gian hiện tại
                project.LastOpened = DateTime.Now;

                // Tải lại danh sách để hiển thị thời gian cập nhật
                LoadProjectsIntoListView();

                // Làm nổi bật mục vừa được cập nhật
                foreach (System.Windows.Forms.ListViewItem item in listView1.Items)
                {
                    if (item.Tag == project)
                    {
                        item.Selected = true;
                        item.EnsureVisible();
                        break;
                    }
                }
                SaveRecentProjects(recentProjects);

                //Load project và vào trang Login
                LoadProjectsIntoListView();
                using (ProgressPopup fr = new ProgressPopup())
                {
                    fr.ShowDialog();
                }
                SystemMode.ProcessStep = 1;
                FrMain.Instance.UIUpdatebtn();
            }
        }
        public class RecentProject
        {
            public string Name { get; set; }
            public string Path { get; set; }
            public DateTime LastOpened { get; set; }
            public bool IsPinned { get; set; }

            public RecentProject(string name, string path, DateTime lastOpened, bool isPinned = false)
            {
                Name = name;
                Path = path;
                LastOpened = lastOpened;
                IsPinned = isPinned;
            }
        }

        private void FrProject_Load(object sender, EventArgs e)
        {
            recentProjects.Clear();
            recentProjects = LoadRecentProjects();
            LoadProjectsIntoListView();
        }

        private void FrProject_Activated(object sender, EventArgs e)
        {
            recentProjects.Clear();
            recentProjects = LoadRecentProjects();
            LoadProjectsIntoListView();
        }

        private void FrProject_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveRecentProjects(recentProjects);
            
        }
    }
}
