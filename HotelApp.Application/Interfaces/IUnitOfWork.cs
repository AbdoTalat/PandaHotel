using HotelApp.Application.Interfaces.IRepositories;
using HotelApp.Domain.Entities;

namespace HotelApp.Application.Interfaces
{
    public interface IUnitOfWork : IAsyncDisposable
    {

		Task<int> CommitAsync(bool skipAuditFields = false, CancellationToken cancellationToken = default);

		Task BeginTransactionAsync();
		Task CommitTransactionAsync();
		Task RollbackTransactionAsync();


        #region Users & Roles (HR / Security)
        IGuestRepository GuestRepository { get; }
        IUserRepository UserRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserBranchRepository UserBranchRepository { get; }
        #endregion

        #region Branches & Companies (Locations)
        IBranchRepository BranchRepository { get; }
        ICompanyRepository CompanyRepository { get; }
        ICountryRepository CountryRepository { get; }
        IStateRepository StateRepository { get; }
        #endregion

        #region Reservations & Sources
        IReservationSourceRepository ReservationSourceRepository { get; }
        IReservationRepository ReservationRepository { get; }
        IReservationRoomRepository ReservationRoomRepository { get; }
        IReservationRoomTypeRepository ReservationRoomTypeRepository { get; }
        IGuestReservationRepository GuestReservationRepository { get; }
        IReservationHistoryRepository ReservationHistoryRepository { get; }
        #endregion

        #region Rooms & Options
        IRoomRepository RoomRepository { get; }
        IRoomTypeRepository RoomTypeRepository { get; }
        IRoomStatusRepository RoomStatusRepository { get; }
        IRoomOptionRepository RoomOptionRepository { get; }
        IOptionRepository OptionRepository { get; }
        IRoomTypeRateRepository RoomTypeRateRepository { get; }
        #endregion

        #region Rates & Calculations
        IRateRepository RateRepository { get; }
        ICalculationTypeRepository CalculationTypeRepository { get; }
        IFloorRepository FloorRepository { get; }
        IProofTypeRepository ProofTypeRepository { get; }
        #endregion

        #region System / Configuration
        ISystemSettingRepositroy SystemSettingRepository { get; }
        #endregion

        IPaymentRepository PaymentRepository { get; }
        IMasterDataItemRepository MasterDataItemRepository { get; }
    }
}
