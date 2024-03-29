/**
* This file is part of Tartarus Emulator.
* 
* Tartarus is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.
* 
* Tartarus is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
* GNU General Public License for more details.
* 
* You should have received a copy of the GNU General Public License
* along with Tartarus.  If not, see<http://www.gnu.org/licenses/>.
*/
using Auth.DataRepository.MySql;
using Auth.DataRepository;
using Auth.DataClasses;

namespace Auth.DataRepository
{
    #region UserRepository
    public class UserRepository
	{
        private IUserDAO _UserDao;

        #region Instance & Assignment
        public UserRepository()
        {
            this._UserDao = new MySql.UserDAO();
        }

		public User GetUser(string id, string password)
		{
            return this._UserDao.Select(id, password);
		}

        public void SaveUser(User user)
        {
            this._UserDao.Update(user);
        }
        #endregion
    }
    #endregion
}

