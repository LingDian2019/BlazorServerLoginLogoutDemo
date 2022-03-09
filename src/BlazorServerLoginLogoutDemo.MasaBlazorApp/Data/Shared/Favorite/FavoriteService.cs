namespace BlazorServerLoginLogoutDemo.MasaBlazorApp.Data.Shared.Favorite
{
    /// <summary>
    /// 收藏服务
    /// </summary>
    public static class FavoriteService
    {
        /// <summary>
        /// 默认收藏的菜单ID
        /// </summary>
        /// <returns></returns>
        public static List<int> GetDefaultFavoriteMenuList() => new() { 11 };
    }
}
