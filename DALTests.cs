using Log_in.mvvm.mvvmDal;
using NUnit.Framework;
using System;

namespace Tests
{
    [TestFixture]
    public class DALTests
    {
        [Test]
        public void TestAjouterUtilisateur_Success()
        {
            // Arrange
            string nom = "John Doe";
            string email = "john@example.com";
            string mdp = "password123";

            // Act
            try
            {
                DAL.CreerUtilisateur(nom, email, mdp);
            }
            catch (Exception ex)
            {
                // Assert
                Assert.Fail($"La création de l'utilisateur a échoué avec l'exception : {ex.Message}");
            }

        }

        [Test]
        public void TestCreerUtilisateur_ExistingEmail_Failure()
        {
            // Arrange
            string nom = "Jane Doe";
            string email = "john@example.com";
            string mdp = "password456";

            // Act
            try
            {
                DAL.CreerUtilisateur(nom, email, mdp);
                // Assert
                Assert.Fail("La création de l'utilisateur avec un e-mail existant a réussi, mais cela ne devrait pas.");
            }
            catch (Exception ex)
            {
               
            }

        }

        [Test]
        public void TestVerifierIdentifiants_CorrectCredentials_Success()
        {
            // Arrange
            string email = "john@example.com";
            string mdp = "password123";

            // Act
            try
            {
                if (!DAL.VerifierIdentifiants(email, mdp))
                    Assert.Fail("La vérification des identifiants a échoué, mais cela ne devrait pas.");
            }
            catch (Exception ex)
            {
                // Assert
                Assert.Fail($"La vérification des identifiants a échoué avec l'exception : {ex.Message}");
            }

        }

        [Test]
        public void TestVerifierIdentifiants_IncorrectCredentials_Failure()
        {
            // Arrange
            string email = "john@example.com";
            string mdp = "wrongpassword";

            // Act
            try
            {
                if (DAL.VerifierIdentifiants(email, mdp))
                    Assert.Fail("La vérification des identifiants a réussi, mais cela ne devrait pas.");
            }
            catch (Exception ex)
            {

            }

        }

    }
}
