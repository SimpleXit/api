using FluentValidation;
using FluentValidation.Validators;
using IbanNet;
using SimpleX.Common.Enums;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.ServiceModel;
using System.Text.RegularExpressions;

namespace SimpleX.Domain.Validation.Helpers
{
    public static class ValidationHelper
    {
        public static IRuleBuilderOptions<T, string> IsValidIban<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            if (ruleBuilder is null)
            {
                throw new ArgumentNullException(nameof(ruleBuilder));
            }

            return ruleBuilder.SetValidator(new SimpleIbanValidator());
        }

        public static IRuleBuilderOptions<T, string> IsValidTaxNumber<T>(this IRuleBuilder<T, string> ruleBuilder, Expression<Func<T, TaxCode>> expression)
        {
            if (ruleBuilder is null)
            {
                throw new ArgumentNullException(nameof(ruleBuilder));
            }

            if (expression is null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            Func<T, TaxCode> func = expression.Compile();

            return ruleBuilder.SetValidator(new SimpleTaxNumberValidator<T>(func));
        }

        public static IRuleBuilderOptions<T, string> IsValidUri<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            if (ruleBuilder is null)
            {
                throw new ArgumentNullException(nameof(ruleBuilder));
            }

            return ruleBuilder.SetValidator(new SimpleUriValidator());
        }
    }

    public class SimpleIbanValidator : PropertyValidator
    {
        private readonly IIbanValidator _ibanValidator;

        public SimpleIbanValidator() : base("Invalid Iban")
        {
            _ibanValidator = new IbanNet.IbanValidator();
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            if (string.IsNullOrEmpty(context?.PropertyValue?.ToString()))
            {
                return true;
            }

            ValidationResult result = _ibanValidator.Validate((string)context.PropertyValue);
            return result.IsValid;
        }
    }

    public class SimpleTaxNumberValidator<T> : PropertyValidator
    {
        private readonly Func<T, TaxCode> _func;
        public SimpleTaxNumberValidator(Func<T, TaxCode> func) : base("Invalid tax number")
        {
            _func = func;
        }

        //<system.serviceModel>
        //  <bindings>
        //    <basicHttpBinding>
        //      <binding name = "checkVatBinding" />
        //    </ basicHttpBinding >
        //  </ bindings >
        //  < client >
        //    < endpoint address="http://ec.europa.eu/taxation_customs/vies/services/checkVatService"
        //      binding="basicHttpBinding" bindingConfiguration="checkVatBinding"
        //      contract="VIES.checkVatPortType" name="checkVatPort" />
        //  </client>
        //</system.serviceModel>

        protected override bool IsValid(PropertyValidatorContext context)
        {
            TaxCode taxCode = _func.Invoke((T)context.InstanceToValidate);
            var inputNumber = context?.PropertyValue?.ToString();

            switch(taxCode)
            {
                case TaxCode.Private:
                    if (string.IsNullOrEmpty(inputNumber))
                        return true;
                    else
                        return false; //TODO: Custom error message
                default:
                    if (string.IsNullOrEmpty(inputNumber))
                        return false;
                    else if (!Regex.IsMatch(inputNumber, "[A-Z]{2}[A-Z0-9]{2,20}"))
                        return false;
                    break;
            }

            BasicHttpBinding bin = new BasicHttpBinding();
            EndpointAddress epa = new EndpointAddress("http://ec.europa.eu/taxation_customs/vies/services/checkVatService");

            try
            {
                var client = new VIES.checkVatPortTypeClient(bin, epa);
               
                string value = context.PropertyValue.ToString().Trim();

                string countryCode = value.Substring(0, 2).ToUpper();
                string secondPart = value.Substring(2).Trim();

                char[] onlyNumbers = secondPart.Where(c => char.IsLetterOrDigit(c)).ToArray();
                string taxNumber = new string(onlyNumbers);

                var result = client.checkVatAsync(new VIES.checkVatRequest(countryCode, taxNumber)).GetAwaiter().GetResult(); ;
                return result.valid;         
            }
            catch(FaultException fe)
            {
                switch(fe.Message)
                {
                    case "INVALID_INPUT":
                        return false;
                    case "INVALID_REQUESTER_INFO":
                        return false;
                    case "SERVICE_UNAVAILABLE":
                        return true;
                    case "MS_UNAVAILABLE":
                        return true;
                    case "TIMEOUT":
                        return true; 
                    case "VAT_BLOCKED":
                        return false;
                    case "IP_BLOCKED":
                        return true;
                    case "GLOBAL_MAX_CONCURRENT_REQ":
                        return true;
                    case "GLOBAL_MAX_CONCURRENT_REQ_TIME":
                        return true;
                    case "MS_MAX_CONCURRENT_REQ":
                        return true;
                    case "MS_MAX_CONCURRENT_REQ_TIME":
                        return true;
                    default:
                        return true;
                }
            }
            catch
            {
                return true;
            }
        }
    }

    public class SimpleUriValidator : PropertyValidator
    {


        public SimpleUriValidator() : base("Invalid uri")
        {
            
        }

        protected override bool IsValid(PropertyValidatorContext context)
        {
            var uri = context?.PropertyValue?.ToString();

            if (string.IsNullOrEmpty(uri))
            {
                return true;
            }

            return Uri.TryCreate(uri, UriKind.Absolute, out _);
        }
    }
}
