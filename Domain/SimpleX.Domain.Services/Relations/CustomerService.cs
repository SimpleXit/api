using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleX.Common.Enums;
using SimpleX.Common.Extensions;
using SimpleX.Data.Context;
using SimpleX.Data.Entities;
using SimpleX.Domain.Models.Relations;
using SimpleX.Domain.Models.Shared;
using SimpleX.Domain.Services.Helpers;
using SimpleX.Helpers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleX.Domain.Services.Relations
{
    public interface ICustomerService : IDisposable
    {
        Task<IEnumerable<CustomerDto>> GetCustomers();
        Task<IEnumerable<CustomerDto>> GetCustomers(CustomerFilterDto filter);

        /// <exception cref="System.Exception"/>
        /// <exception cref="System.InvalidOperationException"/>
        Task<CustomerDto> GetCustomer(long customerId, bool throwIfNotExist = false);

        /// <exception cref="System.Exception"/>
        /// <exception cref="System.InvalidOperationException"/>
        Task<CustomerDto> GetCustomerByNumber(long customerNumber, bool throwIfNotExist = false);

        Task<List<CustomerSearchResultDto>> SearchCustomer(string searchValue);

        Task<CustomerSearchResultDto> SearchCustomerById(long id);

        Task<CustomerDto> InitCustomer();

        /// <exception cref="System.InvalidOperationException"/>
        /// <exception cref="System.ArgumentException"/>
        /// <exception cref="System.ArgumentNullException"/>
        Task<CustomerDto> CreateOrUpdateCustomer(CustomerDto customerIn);

        /// <exception cref="System.Exception"/>
        /// <exception cref="System.InvalidOperationException"/>
        Task DeleteCustomer(long customerId);
        /// <exception cref="System.ArgumentNullException"/>
        Task<CustomerDto> DeleteAddresses(CustomerDto customer, List<AddressDto> addressesToRemove);
        Task<CustomerDto> DeletePersons(CustomerDto customer, List<PersonDto> personsToRemove);
        Task<CustomerDto> DeleteCommunications(CustomerDto customer, List<CommunicationDto> communicationsToRemove);
        Task<CustomerDto> DeleteNotes(CustomerDto customer, List<NoteDto> notesToRemove);
        Task<CustomerDto> DeleteAttachments(CustomerDto customer, List<AttachmentDto> attachmentsToRemove);
        Task<CustomerDto> DeleteAppointments(CustomerDto customer, List<AppointmentDto> appointmentsToRemove);

        Task<List<NoteDto>> GetCustomerNotes(long customerId);
    }

    public class CustomerService : ICustomerService, IDisposable
    {
        private readonly Guid _guid = Guid.NewGuid();
        private readonly ILogger _logger;
        private readonly IDbContextFactory<SimpleContext> _contextFactory;
        private readonly IMapper _mapper;
        private readonly IAuthenticationService _authentication;
        private readonly IValidator<CustomerDto> _customerValidator;
        private readonly DefaultValueSetter _defValSetter;

        private IQueryable<Customer> GetQueryAsNoTracking(SimpleContext context)
        {
            return context.Customer.AsNoTracking()
                                    .Include(c => c.ContactInfo)
                                    .Include(c => c.Addresses)
                                    .Include(c => c.Appointments)
                                    .Include(c => c.Attachments)
                                    .Include(c => c.Communications)
                                    .Include(c => c.Notes)
                                    .Include(c => c.Persons).ThenInclude(p => p.Address)
                                    .Include(c => c.Persons).ThenInclude(p => p.ContactInfo)
                                    .Where(c => c.IsDeleted == false);
        }

        private IQueryable<Customer> GetQueryWithTracking(SimpleContext context)
        {
            return context.Customer.Include(c => c.ContactInfo)
                                    .Include(c => c.Addresses)
                                    .Include(c => c.Appointments)
                                    .Include(c => c.Attachments)
                                    .Include(c => c.Communications)
                                    .Include(c => c.Notes)
                                    .Include(c => c.Persons).ThenInclude(p => p.Address)
                                    .Include(c => c.Persons).ThenInclude(p => p.ContactInfo)
                                    .Where(c => c.IsDeleted == false);
        }

        public CustomerService(
            ILoggerFactory loggerFactory,
            IAuthenticationService authentication,
            IDbContextFactory<SimpleContext> contextFactory,
            IMapper mapper,
            IValidator<CustomerDto> customerValidator,
            DefaultValueSetter defValSetter)
        {
            _logger = loggerFactory?.CreateLogger<CustomerService>() ??
                throw new ArgumentNullException(nameof(loggerFactory));
            _contextFactory = contextFactory ??
                throw new ArgumentNullException(nameof(contextFactory));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _authentication = authentication ??
                throw new ArgumentNullException(nameof(authentication));
            _customerValidator = customerValidator ??
                throw new ArgumentNullException(nameof(customerValidator));
            _defValSetter = defValSetter ??
                throw new ArgumentNullException(nameof(defValSetter));

            _logger.LogTrace("Logger injected in CustomerService");
            _logger.LogTrace("ContextFactory injected in CustomerService");
            _logger.LogTrace("Mapper injected in CustomerService");
            _logger.LogTrace("Authentication injected in CustomerService");
            _logger.LogTrace("CustomerValidator injected in CustomerService");

            _logger.LogTrace("Created CustomerService {0}", _guid);
        }

        public async Task<List<CustomerSearchResultDto>> SearchCustomer(string searchValue)
        {
            try
            {
                _logger.LogTrace("CustomerService.SearchCustomer({0})", searchValue);

                using (var context = _contextFactory.CreateDbContext())
                {
                    var query = from c in context.Customer
                                join ci in context.CustomerContactInfo on c.Id equals ci.CustomerID
                                where !c.IsDeleted
                                  && (c.Number == (searchValue.IsNumeric() ? long.Parse(searchValue) : 0)
                                   || c.FirstName.StartsWith(searchValue)
                                   || c.LastName.StartsWith(searchValue)
                                   || c.ShortName.Contains(searchValue)
                                   || ci.Tel.Contains(searchValue)
                                   || ci.Mail.StartsWith(searchValue)
                                   || context.CustomerAddress.Any(a => a.CustomerID.Value == c.Id
                                                                                && (a.StreetAndNumber.StartsWith(searchValue)
                                                                                 || a.City.StartsWith(searchValue)
                                                                                 || a.ZipCode.Equals(searchValue))))
                                select new CustomerSearchResultDto
                                {
                                    Id = c.Id,
                                    Number = c.Number.ToString("000000"),
                                    FirstName = c.FirstName,
                                    LastName = c.LastName,
                                    TaxNumber = c.TaxNumber
                                };

                    return await query.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CustomerSearchResultDto> SearchCustomerById(long id)
        {
            try
            {
                _logger.LogTrace("CustomerService.SearchCustomerById({0})", id);

                using (var context = _contextFactory.CreateDbContext())
                {
                    var query = from c in context.Customer
                                    //join ci in _context.CustomerContactInfo on c.Id equals ci.CustomerID
                                where !c.IsDeleted
                                   && c.Id == id
                                select new CustomerSearchResultDto
                                {
                                    Id = c.Id,
                                    Number = c.Number.ToString("000000"),
                                    FirstName = c.FirstName,
                                    LastName = c.LastName,
                                    TaxNumber = c.TaxNumber
                                };

                    return await query.FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CustomerDto> InitCustomer()
        {
            try
            {
                _logger.LogTrace("CustomerService.InitCustomer()");

                using (var context = _contextFactory.CreateDbContext())
                {
                    var customer = new CustomerDto()
                    {
                        Number = await GetNextCustomerNumber(context)
                    };

                    var defaults = await context.DefaultValue.AsNoTracking()
                                                              .Where(d => d.EntityType == EntityType.Customer)
                                                              .ToListAsync();

                    _defValSetter.SetValues(defaults, customer);

                    customer.Init();
                    return customer;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<CustomerDto>> GetCustomers()
        {
            try
            {
                _logger.LogTrace($"CustomerService.GetCustomers()");

                using (var context = _contextFactory.CreateDbContext())
                {
                    var customers = await GetQueryAsNoTracking(context).ToListAsync();

                    return _mapper.Map<IEnumerable<CustomerDto>>(customers);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<CustomerDto>> GetCustomers(CustomerFilterDto filter)
        {
            try
            {
                _logger.LogTrace($"CustomerService.GetCustomers({filter.ToString()})");

                using (var context = _contextFactory.CreateDbContext())
                {
                    var qry = GetQueryAsNoTracking(context);

                    if (!string.IsNullOrEmpty(filter.FirstName))
                        qry.Where(c => c.FirstName.StartsWith(filter.FirstName));

                        
                    var customers = await qry.ToListAsync();

                    return _mapper.Map<IEnumerable<CustomerDto>>(customers);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CustomerDto> GetCustomer(long customerId, bool throwIfNotExist = false)
        {
            try
            {
                _logger.LogTrace($"CustomerService.GetCustomer({customerId})");

                using (var context = _contextFactory.CreateDbContext())
                {
                    var customer = await GetQueryAsNoTracking(context).FirstOrDefaultAsync(c => c.Id == customerId);

                    if (customer == null)
                    {
                        if (throwIfNotExist)
                            throw new InvalidOperationException($"CustomerId {customerId} not found.");
                        else
                            return null;
                    }

                    return _mapper.Map<CustomerDto>(customer);
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CustomerDto> GetCustomerByNumber(long customerNumber, bool throwIfNotExist = false)
        {
            try
            {
                _logger.LogTrace($"CustomerService.GetCustomerByNumber({customerNumber})");

                using (var context = _contextFactory.CreateDbContext())
                {
                    var customer = await GetQueryAsNoTracking(context).FirstOrDefaultAsync(c => c.Number == customerNumber);

                    if (customer == null)
                    {
                        if (throwIfNotExist)
                            throw new InvalidOperationException($"CustomerNumber {customerNumber} not found.");
                        else
                            return null;
                    }

                    return _mapper.Map<CustomerDto>(customer);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CustomerDto> CreateOrUpdateCustomer(CustomerDto customerIn)
        {
            try
            {
                if (customerIn == null)
                    throw new ArgumentNullException(nameof(customerIn));

                var vr = _customerValidator.Validate(customerIn);
                if (!vr.IsValid)
                    throw new ArgumentException(vr.Errors.ToString());

                if (customerIn.Id == 0)
                    return await CreateCustomer(customerIn);
                else
                    return await UpdateCustomer(customerIn);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }

        private async Task<CustomerDto> CreateCustomer(CustomerDto customerIn)
        {
            try
            {
                _logger.LogTrace("CustomerService.CreateCustomer({0})", customerIn);

                using (var context = _contextFactory.CreateDbContext())
                {
                    if (context.Customer.AsNoTracking().Any(c => c.Number == customerIn.Number))
                    {
                        throw new InvalidOperationException($"CustomerNumber {customerIn.Number} already exists.");
                    }

                    var customer = _mapper.Map<Customer>(customerIn);

                    context.Customer.Add(customer);
                    await context.SaveChangesAsync();

                    return _mapper.Map(customer, customerIn);
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<CustomerDto> UpdateCustomer(CustomerDto customerIn)
        {
            try
            {
                _logger.LogTrace("CustomerService.UpdateCustomer({0},{1})", customerIn.Id, customerIn);

                using (var context = _contextFactory.CreateDbContext())
                { 
                    var customer = await GetQueryWithTracking(context).FirstOrDefaultAsync(c => c.Id == customerIn.Id);

                    if (customer == null)
                    {
                        throw new InvalidOperationException($"CustomerId {customerIn.Id} not found.");
                    }

                    //Map dto values to entity:
                    _mapper.Map(customerIn, customer);

                    //_context.Update not needed because customer is being tracked by context
                    //_context.Update(customer);

                    if (context.IsDirty())
                        await context.SaveChangesAsync();

                    //Map entity back to dto:
                    return _mapper.Map(customer, customerIn);
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new DbUpdateConcurrencyException($"CustomerId {customerIn.Id} has been altered by an other user: {ex.Message}.");
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new DbUpdateConcurrencyException($"CustomerId {customerIn.Id} could not be updated in db: {ex.Message}.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteCustomer(long customerId)
        {
            try
            {
                _logger.LogTrace($"CustomerService.DeleteCustomer({customerId})");

                using (var context = _contextFactory.CreateDbContext())
                {
                    var customer = await GetQueryAsNoTracking(context).FirstOrDefaultAsync(c => c.Id == customerId);

                    if (customer == null)
                    {
                        throw new InvalidOperationException($"CustomerId {customerId} not found.");
                    }

                    //TODO: Test
                    var hasHistory = await context.Document.AsNoTracking()
                                                            .Include(d => d.Associate)
                                                            .Where(d => d.Associate.CustomerID == customerId)
                                                            .AnyAsync();

                    if (hasHistory)
                    {
                        customer.IsDeleted = true;
                        customer.DeletedOn = DateTime.Now;
                        customer.DeletedBy = _authentication.CurrentUsername;
                        context.Update(customer);
                    }
                    else
                    {
                        //TODO: Customer with appointments?
                        context.RemoveRange(customer.Addresses);
                        context.RemoveRange(customer.Attachments);
                        context.RemoveRange(customer.Communications);
                        context.RemoveRange(customer.Notes);
                        context.RemoveRange(customer.Persons);
                        context.Remove(customer.ContactInfo);
                        context.Remove(customer);
                    }

                    await context.SaveChangesAsync();
                }
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<long> GetNextCustomerNumber(SimpleContext context)
        {
            try
            {
                var number = from c1 in context.Customer
                             orderby c1.Number
                             where c1.Number > 100000 && c1.Number < 999999
                                && !(context.Customer.Any(c2 => c2.Number == c1.Number + 1))
                             select c1.Number;

                var result = await number.FirstOrDefaultAsync();

                if (result == 0)
                    return 100001;
                else
                    return result + 1;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<CustomerDto> DeleteAddresses(CustomerDto customer, List<AddressDto> addressesToRemove)
        {
            try
            {
                if (customer == null)
                    throw new ArgumentNullException(nameof(customer));

                if (addressesToRemove == null)
                    throw new ArgumentNullException(nameof(addressesToRemove));

                if (addressesToRemove.Count == 0)
                    return customer;

                if (addressesToRemove.Any(a => a.AddressType == AddressType.Default))
                    throw new InvalidOperationException("Default address can not be removed.");

                using (var context = _contextFactory.CreateDbContext())
                {
                    var arrayOfAddressIdsToRemove = addressesToRemove.Select(a => a.Id).ToArray();
                    var addresses = await context.CustomerAddress.Where(a => arrayOfAddressIdsToRemove.Contains(a.Id)).ToListAsync();

                    context.RemoveRange(addresses);
                    await context.SaveChangesAsync();
                }

                foreach(AddressDto addressToRemove in addressesToRemove)
                {
                    customer.Addresses.Remove(addressToRemove);
                }

                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CustomerDto> DeletePersons(CustomerDto customer, List<PersonDto> personsToRemove)
        {
            try
            {
                if (customer == null)
                    throw new ArgumentNullException(nameof(customer));

                if (personsToRemove == null)
                    throw new ArgumentNullException(nameof(personsToRemove));

                if (personsToRemove.Count == 0)
                    return customer;

                using (var context = _contextFactory.CreateDbContext())
                {
                    var arrayOfPersonIdsToRemove = personsToRemove.Select(a => a.Id).ToArray();
                    var persons = await context.CustomerPerson.Where(a => arrayOfPersonIdsToRemove.Contains(a.Id)).ToListAsync();

                    context.RemoveRange(persons);
                    await context.SaveChangesAsync();
                }

                foreach (PersonDto personToRemove in personsToRemove)
                {
                    customer.Persons.Remove(personToRemove);
                }

                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CustomerDto> DeleteNotes(CustomerDto customer, List<NoteDto> notesToRemove)
        {
            try
            {
                if (customer == null)
                    throw new ArgumentNullException(nameof(customer));

                if (notesToRemove == null)
                    throw new ArgumentNullException(nameof(notesToRemove));

                if (notesToRemove.Count == 0)
                    return customer;

                using (var context = _contextFactory.CreateDbContext())
                {
                    var arrayOfNoteIdsToRemove = notesToRemove.Select(a => a.Id).ToArray();
                    var notes = await context.CustomerNote.Where(a => arrayOfNoteIdsToRemove.Contains(a.Id)).ToListAsync();

                    context.RemoveRange(notes);
                    await context.SaveChangesAsync();
                }

                foreach (NoteDto noteToRemove in notesToRemove)
                {
                    customer.Notes.Remove(noteToRemove);
                }

                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CustomerDto> DeleteCommunications(CustomerDto customer, List<CommunicationDto> communicationsToRemove)
        {
            try
            {
                if (customer == null)
                    throw new ArgumentNullException(nameof(customer));

                if (communicationsToRemove == null)
                    throw new ArgumentNullException(nameof(communicationsToRemove));

                if (communicationsToRemove.Count == 0)
                    return customer;

                using (var context = _contextFactory.CreateDbContext())
                {
                    var arrayOfCommunicationIdsToRemove = communicationsToRemove.Select(a => a.Id).ToArray();
                    var communications = await context.CustomerCommunication.Where(a => arrayOfCommunicationIdsToRemove.Contains(a.Id)).ToListAsync();

                    context.RemoveRange(communications);
                    await context.SaveChangesAsync();
                }

                foreach (CommunicationDto communicationToRemove in communicationsToRemove)
                {
                    customer.Communications.Remove(communicationToRemove);
                }

                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CustomerDto> DeleteAttachments(CustomerDto customer, List<AttachmentDto> attachmentsToRemove)
        {
            try
            {
                if (customer == null)
                    throw new ArgumentNullException(nameof(customer));

                if (attachmentsToRemove == null)
                    throw new ArgumentNullException(nameof(attachmentsToRemove));

                if (attachmentsToRemove.Count == 0)
                    return customer;

                using (var context = _contextFactory.CreateDbContext())
                {
                    var arrayOfAttachmentIdsToRemove = attachmentsToRemove.Select(a => a.Id).ToArray();
                    var attachments = await context.CustomerAttachment.Where(a => arrayOfAttachmentIdsToRemove.Contains(a.Id)).ToListAsync();

                    context.RemoveRange(attachments);
                    await context.SaveChangesAsync();
                }

                foreach (AttachmentDto attachmentToRemove in attachmentsToRemove)
                {
                    customer.Attachments.Remove(attachmentToRemove);
                }

                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CustomerDto> DeleteAppointments(CustomerDto customer, List<AppointmentDto> appointmentsToRemove)
        {
            try
            {
                if (customer == null)
                    throw new ArgumentNullException(nameof(customer));

                if (appointmentsToRemove == null)
                    throw new ArgumentNullException(nameof(appointmentsToRemove));

                if (appointmentsToRemove.Count == 0)
                    return customer;

                using (var context = _contextFactory.CreateDbContext())
                {
                    var arrayOfAppointmentIdsToRemove = appointmentsToRemove.Select(a => a.Id).ToArray();
                    var appointments = await context.Appointment.Where(a => arrayOfAppointmentIdsToRemove.Contains(a.Id)).ToListAsync();

                    context.RemoveRange(appointments);
                    await context.SaveChangesAsync();
                }

                foreach (AppointmentDto appointmentToRemove in appointmentsToRemove)
                {
                    customer.Appointments.Remove(appointmentToRemove);
                }

                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<List<NoteDto>> GetCustomerNotes(long customerId)
        {
            try
            {
                using (var context = _contextFactory.CreateDbContext())
                {
                    var notes = await context.CustomerNote.Where(n => n.CustomerID == customerId).ToListAsync();

                    return _mapper.Map<List<NoteDto>>(notes);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
                throw;
            }
        }

        #region IDisposable Members

        private bool disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposed || !disposing)
                return;

            _logger.LogTrace("Disposing CustomerService {0}", _guid);

            disposed = true;
        }

        #endregion
    }
}
