import { useState, useEffect } from 'react';
import React from 'react';
import { TableFooter,TablePagination, Table, TableHead, TableCell, TableRow, TableBody, Button, styled, Container } from '@mui/material'
import { getUsers, deleteUser } from '../../Service/UserApi';
import { Link } from 'react-router-dom';
import LocalizedStrings from 'react-localization';
import strings from '../../localization.json'

const StyledTable = styled(Table)`
    width: 90%;
    margin: 20px 0 0px 50px;
    
`;

const StyledButton = styled(Button)`
    max-width:200px
`;

const TContainer = styled(Container)`
   justify-content: flex-end;
   display: flex;
   max-width:90% !important;
   margin: 20px 0 0 50px !important;
   padding:0 !important;
   align-items: flex-end !important;
`;

const THead = styled(TableRow)`
    & > th {
        font-size: 20px;
        background: #000000;
        color: #FFFFFF;
    }
`;

const TRow = styled(TableRow)`
    & > td{
        font-size: 18px
    }
`;


const AllUsers = () => {
    const [users, setUsers] = useState([]);
    const [page, setPage] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(5);
    const [language] = useState(localStorage.getItem('language') || 'en'); // Default language is English
    const [localization] = useState(()=>{
          return new LocalizedStrings(strings);
  
    });
    localization.setLanguage(language);

    useEffect(() => {
        getAllUsers();
    }, []);
    const handleChangePage = (event, newPage) => {
        setPage(newPage);
      };
    
      const handleChangeRowsPerPage = (event) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
      };
    const deleteUserData = async (id) => {
        await deleteUser(id);
        getAllUsers();
    }

    const getAllUsers = async () => {
        let response = await getUsers();
        if(response !==undefined)
            setUsers(response.data);
    }

    return (

        <div>
            <TContainer maxWidth="1200">
                <StyledButton variant = "contained" href="users/add">{localization.addUserLabel}</StyledButton>
            </TContainer>
            
        <StyledTable>
            
            <TableHead>
                <THead>
                    <TableCell>Id</TableCell>
                    <TableCell>{localization.firstNameLabel}</TableCell>
                    <TableCell>{localization.lastNameLabel}</TableCell>
                    <TableCell>Username</TableCell>
                    <TableCell>{localization.jobLabel}</TableCell>
                    <TableCell>Email</TableCell>
                    <TableCell>{localization.phoneLabel}</TableCell>
                    <TableCell></TableCell>
                </THead>
            </TableHead>
            <TableBody>
                {(rowsPerPage > 0
            ? users.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
            : users
          ).map((user) => (
                    <TRow key={user.id}>
                        <TableCell>{user.id}</TableCell>
                        <TableCell>{user.first_Name}</TableCell>
                        <TableCell>{user.last_Name}</TableCell>
                        <TableCell>{user.userName}</TableCell>
                        <TableCell>{user.jobTitle}</TableCell>
                        <TableCell>{user.email}</TableCell>
                        <TableCell>{user.phoneNumber}</TableCell>
                        <TableCell>
                            <Button color="primary" variant="contained" style={{marginRight:10}} component={Link} to={`edit/${user.id}`}>{localization.editLabel}</Button>
                            <Button color="secondary" variant="contained" onClick={() => deleteUserData(user.id)}>{localization.deleteLabel}</Button> 
                        </TableCell>
                    </TRow>
                ))}
            </TableBody>
            <TableFooter>
                <TableRow>
                    <TablePagination
                        rowsPerPageOptions={[5, 10, 25, 50, { label: 'All', value: -1 }]}
                        colSpan={8}
                        count={users.length}
                        rowsPerPage={rowsPerPage}
                        page={page}
                        SelectProps={{
                            inputProps: {
                            'aria-label': 'rows per page',
                            },
                            native: true,
                        }}
                        onPageChange={handleChangePage}
                        onRowsPerPageChange={handleChangeRowsPerPage}
                        />
              </TableRow>
            </TableFooter>
        </StyledTable>
        </div>
    )
}

export default AllUsers;