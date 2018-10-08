using SchibstedBackendTest.Areas.HelpPage.ModelDescriptions;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Web.Http.Description;

namespace SchibstedBackendTest.Areas.HelpPage.Models
{
    public class HelpPageApiModel
    {
        public HelpPageApiModel()
        {
            UriParameters = new Collection<ParameterDescription>();
            SampleRequests = new Dictionary<MediaTypeHeaderValue, object>();
            SampleResponses = new Dictionary<MediaTypeHeaderValue, object>();
            ErrorMessages = new Collection<string>();
        }

        public ApiDescription ApiDescription { get; set; }

        public Collection<ParameterDescription> UriParameters { get; private set; }

        public string RequestDocumentation { get; set; }

        public ModelDescription RequestModelDescription { get; set; }

        public IList<ParameterDescription> RequestBodyParameters
        {
            get
            {
                return GetParameterDescriptions(RequestModelDescription);
            }
        }

        public ModelDescription ResourceDescription { get; set; }

        public IList<ParameterDescription> ResourceProperties
        {
            get
            {
                return GetParameterDescriptions(ResourceDescription);
            }
        }

        public IDictionary<MediaTypeHeaderValue, object> SampleRequests { get; private set; }

        public IDictionary<MediaTypeHeaderValue, object> SampleResponses { get; private set; }

        public Collection<string> ErrorMessages { get; private set; }

        private static IList<ParameterDescription> GetParameterDescriptions(ModelDescription modelDescription)
        {
            ComplexTypeModelDescription complexTypeModelDescription = modelDescription as ComplexTypeModelDescription;
            if (complexTypeModelDescription != null)
            {
                return complexTypeModelDescription.Properties;
            }

            CollectionModelDescription collectionModelDescription = modelDescription as CollectionModelDescription;
            if (collectionModelDescription != null)
            {
                complexTypeModelDescription = collectionModelDescription.ElementDescription as ComplexTypeModelDescription;
                if (complexTypeModelDescription != null)
                {
                    return complexTypeModelDescription.Properties;
                }
            }

            return null;
        }
    }
}