using AmusementPark.Application.Interfaces;
using AmusementPark.Domain.Entities;
using AmusementPark.Domain.Interfaces;

namespace AmusementPark.Application.Services
{
    /// <summary>
    /// منطق تجاری مربوط به Player.
    ///
    /// این کلاس به Interface وابسته است،
    /// نه به پیاده‌سازی مستقیم Repository.
    /// </summary>
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayerService(
            IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        public List<Player> GetAll()
        {
            return _playerRepository.GetAll();
        }

        public Player? GetById(int id)
        {
            return _playerRepository.GetById(id);
        }

        public Player? GetByMobile(string mobile)
        {
            return _playerRepository.GetByMobile(mobile);
        }

        public Player? GetByName(string playerName)
        {
            return _playerRepository.GetByName(playerName);
        }

        public Player? GetByNameAndMobile(
            string playerName,
            string mobile)
        {
            return _playerRepository.GetByNameAndMobile(
                playerName,
                mobile);
        }

        public void Add(Player player)
        {
            if (string.IsNullOrWhiteSpace(player.PlayerName))
            {
                throw new ArgumentException(
                    "نام کاربری الزامی است.");
            }

            if (string.IsNullOrWhiteSpace(player.PlayerMobile))
            {
                throw new ArgumentException(
                    "شماره موبایل الزامی است.");
            }

            _playerRepository.Add(player);
        }

        public void Update(Player player)
        {
            if (player.PlayerID == null)
            {
                throw new ArgumentException(
                    "شناسه کاربر معتبر نیست.");
            }

            _playerRepository.Update(player);
        }

       
        public void Delete(int id)
        {
            _playerRepository.Delete(id);
        }
    }
}