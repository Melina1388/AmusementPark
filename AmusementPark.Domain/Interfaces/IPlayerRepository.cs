using AmusementPark.Domain.Entities;

namespace AmusementPark.Domain.Interfaces
{
    /// <summary>
    /// قرارداد دسترسی به اطلاعات Player.
    ///
    /// Domain نباید بداند دیتابیس از چه تکنولوژی‌ای استفاده می‌کند.
    /// پیاده‌سازی این Interface در Infrastructure قرار دارد.
    /// </summary>
    public interface IPlayerRepository
    {
        /// <summary>
        /// دریافت تمام کاربران.
        /// </summary>
        List<Player> GetAll();

        /// <summary>
        /// دریافت کاربر بر اساس شناسه.
        /// </summary>
        Player? GetById(int id);

        /// <summary>
        /// دریافت کاربر بر اساس شماره موبایل.
        /// </summary>
        Player? GetByMobile(string mobile);

        /// <summary>
        /// دریافت کاربر بر اساس نام کاربری.
        /// </summary>
        Player? GetByName(string playerName);

        /// <summary>
        /// دریافت کاربر بر اساس نام کاربری و شماره موبایل.
        /// </summary>
        Player? GetByNameAndMobile(
            string playerName,
            string mobile);

        /// <summary>
        /// ثبت کاربر جدید در دیتابیس.
        /// </summary>
        void Add(Player player);

        /// <summary>
        /// ویرایش اطلاعات کاربر.
        /// </summary>
        void Update(Player player);

        /// <summary>
        /// حذف کاربر.
        /// </summary>
        void Delete(int id);
    }
}