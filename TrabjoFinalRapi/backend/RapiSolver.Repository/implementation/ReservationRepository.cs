using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using RapiSolver.Entity;
using RapiSolver.Repository.context;
using RapiSolver.Repository.ViewModel;

namespace RapiSolver.Repository.implementation
{
    public class ReservationRepository : IReservationRepository
    {
        private ApplicationDbContext context;

        public ReservationRepository (ApplicationDbContext context) {
            this.context = context;
        }

        public bool Delete(int id)
        {
            try
            {
                Reservation r1=context.reservations.Find(id);
                context.Remove(r1);
                context.SaveChanges();
            }
            catch (System.Exception)
            {

                return false;
            }
            return true;
        }

        public Reservation Get(int id)
        {
           var result = new Reservation();
           try{
               result = context.reservations.Single(x => x.ReservationId == id);
           }
           catch(System.Exception){
               throw;
           }
           return result;
        }

        public IEnumerable<Reservation> GetAll()
        {
            var result = new List<Reservation>();
           try{
               result = context.reservations.ToList();

           }
           catch(System.Exception){
               throw;
           }
           return result;
        }

        public IEnumerable<ReservationViewModel> GetAllReservationsByUserId(int id)
        {
            Usuario u1=context.usuarios.Find(id);

            if(u1.RolId==2){
                    Supplier supplier=context.suppliers.Where(x=>x.UsuarioId==id).First();
                 var reservations = context.reservations
                .Include (o => o.Supplier)
                .Include(o=>o.Usuario)
                .Include(o=>o.Servicio)
                .Include(o=>o.Servicio.ServiceCategory)
                .Where(o=>o.UsuarioId==id || o.SupplierId==supplier.SupplierId)
                .ToList ();

                return reservations.Select (o => new ReservationViewModel {
                    ReservationId = o.ReservationId,
                    ServicioId = o.ServicioId,
                    ServicioName = o.Servicio.Name,
                    ServicioCategory = o.Servicio.ServiceCategory.CategoryName,
                    UsuarioId=o.UsuarioId,
                    UsuarioName=o.Usuario.UserName,
                    UsuarioLastName=o.Usuario.UserName,
                    SupplierId=o.SupplierId,
                    SupplierName=o.Supplier.Name,
                    SupplierLastName=o.Supplier.LastName,
                    Fecha=o.Fecha,
                    Note=o.Note
                  
             });
            }else{
                var reservations = context.reservations
                .Include (o => o.Supplier)
                .Include(o=>o.Usuario)
                .Include(o=>o.Servicio)
                .Include(o=>o.Servicio.ServiceCategory)
                .Where(o=>o.UsuarioId==id )
                .ToList ();

                return reservations.Select (o => new ReservationViewModel {
                    ReservationId = o.ReservationId,
                    ServicioId = o.ServicioId,
                    ServicioName = o.Servicio.Name,
                    ServicioCategory = o.Servicio.ServiceCategory.CategoryName,
                    UsuarioId=o.UsuarioId,
                    UsuarioName=o.Usuario.UserName,
                    UsuarioLastName=o.Usuario.UserName,
                    SupplierId=o.SupplierId,
                    SupplierName=o.Supplier.Name,
                    SupplierLastName=o.Supplier.LastName,
                    Fecha=o.Fecha,
                    Note=o.Note
                  
             });
            }
           
            
        }

        public bool Save(Reservation entity)
        {
            try
            {
                Usuario u1=context.usuarios.Where(x=>x.UsuarioId==entity.UsuarioId).First();
                Servicio s1=context.servicios.Where(x=>x.ServicioId==entity.ServicioId).First();
                Supplier su1=context.suppliers.Where(x=>x.SupplierId==entity.SupplierId).First();

                entity.Usuario=u1;
                entity.Servicio=s1;
                entity.Supplier=su1;

                context.Add(entity);
                context.SaveChanges();
            }
            catch (System.Exception)
            {

                return false;
            }
            return true;
        }

        public bool Update(Reservation entity)
        {
            throw new System.NotImplementedException();
        }
    }
}