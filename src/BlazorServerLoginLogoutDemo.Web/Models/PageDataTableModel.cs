using System.Diagnostics;

namespace BlazorServerLoginLogoutDemo.Web.Models
{
    /// <summary>
    /// 页Table列表Model
    /// </summary>
    /// <typeparam name="TableShowModel">列表显示Model</typeparam>
    /// <typeparam name="QueryModel">表单查询Model</typeparam>
    public class PageDataTableModel<TableShowModel, QueryModel> where QueryModel : new() where TableShowModel : new()
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public PageDataTableModel()
        {
            ModelQuery = new QueryModel();
            ModelList = new List<TableShowModel>();

            QueryElapsedMilliseconds = 0;
            PageIndexInitial = 1;
            PageSizeInitial = 10;
            Total = 0;
            PageIndex = PageIndexInitial;
            PageSize = PageSizeInitial;
            Loading = true;
        }

        #region 字段
        /// <summary>
        /// 用于查询记时间
        /// </summary>
        public Stopwatch? QueryStopwatch { get; private set; }

        /// <summary>
        /// 查询经过多少秒
        /// </summary>
        public long QueryElapsedMilliseconds { get; private set; }

        /// <summary>
        /// 列表显示Model
        /// </summary>
        public QueryModel ModelQuery { get; set; }

        /// <summary>
        /// 表单查询Model
        /// </summary>
        public IEnumerable<TableShowModel> ModelList { get; set; }

        /// <summary>
        /// 总条数
        /// </summary>
        public int Total { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int PageCount => (int)Math.Ceiling(Total / (double)PageSize);

        /// <summary>
        /// 分页索引
        /// </summary>
        public int PageIndex { get; set; }

        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 分页索引初始值
        /// </summary>
        public int PageIndexInitial { get; set; }

        /// <summary>
        /// 页大小初始值
        /// </summary>
        public int PageSizeInitial { get; set; }

        /// <summary>
        /// 显示在加载中
        /// </summary>
        public bool Loading { get; set; }

        /// <summary>
        /// Table 页脚 显示内容
        /// </summary>
        public string DataTableFooter
        {
            get
            {
                if (QueryElapsedMilliseconds == 0)
                {
                    return string.Empty;
                }
                else
                {
                    return $"数据加载耗时：{QueryElapsedMilliseconds}/毫秒";
                }
            }
        }

        /// <summary>
        /// 页大小模板
        /// </summary>
        public List<int> PageSizesTemplate = new() { 10, 25, 50, 100 };
        #endregion

        /// <summary>
        /// 启动计时器
        /// </summary>
        public void StartTimer()
        {
            QueryStopwatch = new Stopwatch();
            QueryStopwatch.Start();
            Loading = true;
            Total = 0;
        }

        /// <summary>
        /// 停止计时器
        /// </summary>
        public void EndTimer()
        {
            // 停止计时器信息并计算时间
            if(QueryStopwatch != null)
            {
                QueryStopwatch.Stop();
                QueryElapsedMilliseconds = QueryStopwatch.ElapsedMilliseconds;
            }
           
            QueryStopwatch = null;
            Loading = false;
        }
    }
}
