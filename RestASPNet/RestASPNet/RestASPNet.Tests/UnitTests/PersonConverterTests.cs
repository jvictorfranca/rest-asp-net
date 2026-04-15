using FluentAssertions;
using RestASPNet.Data.Converter.Impl;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace RestASPNet.Tests.UnitTests
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
        public void Parse_NullPersonDTOShouldReturnNull()
        {
            PersonDTO personDTO = null;

            // Act

            var resultPerson = _converter.Parse(personDTO);

            // Assert

            resultPerson.Should().BeNull();
        }

        // Person Should parse

        [Fact]
        public void Parse_ShouldConvertPersonToPersonDTO()
        {
            // Arrange
            var person = new Person
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                Adress = "123 Main"
            };

            var expectedPersonDTO = new PersonDTO
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Gender = "Male",
                Adress = "123 Main"
            };

            // Act

            var resultPerson = _converter.Parse(person);

            // Assert

            resultPerson.Should().NotBeNull();
            resultPerson.Id.Should().Be(expectedPersonDTO.Id);
            resultPerson.FirstName.Should().Be(expectedPersonDTO.FirstName);
            resultPerson.LastName.Should().Be(expectedPersonDTO.LastName);
            resultPerson.Gender.Should().Be(expectedPersonDTO.Gender);
            resultPerson.Adress.Should().Be(expectedPersonDTO.Adress);

            resultPerson.Should().BeEquivalentTo(expectedPersonDTO, options => options.Excluding(person => person.Gender));

        }

        [Fact]
        public void Parse_NullPersonShouldReturnNull()
        {
            // Arrange
            Person person = null;

            // Act

            var resultPerson = _converter.Parse(person);

            // Assert

            resultPerson.Should().BeNull();
        }

        [Fact]
        public void ParseList_ShouldConvertPersonDTOListToPersonList()
        {
            // Arrange

            var dtoList = new List<PersonDTO>
            {
                new PersonDTO
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Gender = "Male",
                    Adress = "123 Main"
                },
                new PersonDTO
                {
                    Id = 2,
                    FirstName = "John2",
                    LastName = "Doe2",
                    Gender = "Male2",
                    Adress = "123 Main2"
                }
            };

            var expectedPersonList = new List<Person>
            {
                new Person
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Gender = "Male",
                    Adress = "123 Main"
                },
                new Person
                {
                    Id = 2,
                    FirstName = "John2",
                    LastName = "Doe2",
                    Gender = "Male2",
                    Adress = "123 Main2"
                }
            };

            // Act

            var personList = _converter.ParseList(dtoList);

            // Assert

            personList.Should().NotBeNull();
            personList.Should().HaveCount(2);
            personList.Should().BeEquivalentTo(expectedPersonList);
            personList[0].LastName.Should().Be("Doe");

        }

        [Fact]
        public void ParseList_ShouldConvertPersonListToPersonDTOList()
        {
            // Arrange

            var dtoList = new List<Person>
            {
                new Person
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Gender = "Male",
                    Adress = "123 Main"
                },
                new Person
                {
                    Id = 2,
                    FirstName = "John2",
                    LastName = "Doe2",
                    Gender = "Male2",
                    Adress = "123 Main2"
                }
            };

            var expectedPersonList = new List<PersonDTO>
            {
                new PersonDTO
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Gender = "Male",
                    Adress = "123 Main"
                },
                new PersonDTO
                {
                    Id = 2,
                    FirstName = "John2",
                    LastName = "Doe2",
                    Gender = "Male2",
                    Adress = "123 Main2"
                }
            };

            // Act

            var personList = _converter.ParseList(dtoList);

            // Assert

            personList.Should().NotBeNull();
            personList.Should().HaveCount(2);
            personList.Should().BeEquivalentTo(expectedPersonList);

        }
    }
}
