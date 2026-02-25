using FluentAssertions;
using RestASPNet.Controllers.Model;
using RestASPNet.Data.Converter.Impl;
using RestASPNet.Data.DTO.V1;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestASPNet.Tests
{
    public class PersonConverterTests
    {
        private readonly PersonConverterV1 _converter;

        public PersonConverterTests()
        {
            _converter = new PersonConverterV1();
        }

        // PersonDTO Should parse

        [Fact]
        public void Parse_ShouldConvertPersonDTOToPersonEntity()
        {
            // Arrange
            var personDTO = new PersonDTO
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                Adress = "123 Main"
            };

            var expectedPerson = new Person
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                Adress = "123 Main"
            };

            // Act

            var resultPerson = _converter.Parse(personDTO);

            // Assert

            resultPerson.Should().NotBeNull();
            resultPerson.Id.Should().Be(expectedPerson.Id);
            resultPerson.FirstName.Should().Be(expectedPerson.FirstName);
            resultPerson.LastName.Should().Be(expectedPerson.LastName);
            resultPerson.Gender.Should().Be(expectedPerson.Gender);
            resultPerson.Adress.Should().Be(expectedPerson.Adress);

            resultPerson.Should().BeEquivalentTo(expectedPerson);

        }

        [Fact]
        public void Parse_SNullPersonDTOShouldReturnNull()
        {
            PersonDTO personDTO = null;

            // Act

            var resultPerson = _converter.Parse(personDTO);

            // Assert

            resultPerson.Should().BeNull();
        }
    }
}
