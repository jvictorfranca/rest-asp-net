using RestASPNet.Data.DTO.V1;

namespace RestASPNet.Services
{
    public interface IPersonServices
    {
        PersonDTO Create(PersonDTO person);
        PersonDTO FindById(long id);
        List<PersonDTO> FindAll();
        PersonDTO Update(PersonDTO person);
        void Delete(long id);

        PersonDTO Disable(long id);
    }
}
