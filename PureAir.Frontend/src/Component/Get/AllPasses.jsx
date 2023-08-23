import { useState, useEffect } from 'react';
import React from 'react';
import { TableFooter,TablePagination, Table, TableHead, TableCell, TableRow, TableBody, Button, styled, Container } from '@mui/material'
import { getPasses, deletePass } from '../../Service/PassApi';
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


const AllPasses = () => {
    const [passes, setPasses] = useState([]);
    const [page, setPage] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(5);
    const [language] = useState(localStorage.getItem('language') || 'en'); // Default language is English
    const [localization] = useState(()=>{
          return new LocalizedStrings(strings);
  
    });
    localization.setLanguage(language);

    useEffect(() => {
        getAllPasses();
    }, []);

    const handleChangePage = (event, newPage) => {
        setPage(newPage);
      };
    
      const handleChangeRowsPerPage = (event) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
      };
    const deletePassData = async (id) => {
        await deletePass(id);
        getAllPasses();
    }

    const getAllPasses = async () => {
        let response = await getPasses();
        if(response !==undefined)
            setPasses(response.data);
    }

    return (

        <div>
            <TContainer maxWidth="1200">
                <StyledButton variant = "contained" href="passes/add">{localization.addPassLabel}</StyledButton>
            </TContainer>
            
        <StyledTable>
            
            <TableHead>
                <THead>
                    <TableCell>{localization.passIdLabel}</TableCell>
                    <TableCell>{localization.employeeIdLabel}</TableCell>
                    <TableCell>{localization.lastWorkspaceLabel}</TableCell>
                    <TableCell>{localization.timestampLabel}</TableCell>
                    <TableCell></TableCell>
                </THead>
            </TableHead>
            <TableBody>
                {(rowsPerPage > 0
            ? passes.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
            : passes
          ).map((pass) => (
                    <TRow key={pass.id}>
                        <TableCell>{pass.id}</TableCell>
                        <TableCell>{pass.employeeId}</TableCell>
                        <TableCell>{pass.workspaceId}</TableCell>
                        <TableCell>{pass.timestamp}</TableCell>
                        <TableCell>
                            <Button color="primary" variant="contained" style={{marginRight:10}} component={Link} to={`edit/${pass.id}`}>{localization.editLabel}</Button>
                            <Button color="secondary" variant="contained" onClick={() => deletePassData(pass.id)}>{localization.deleteLabel}</Button> 
                        </TableCell>
                    </TRow>
                ))}
            </TableBody>
            <TableFooter>
                <TableRow>
                    <TablePagination
                        rowsPerPageOptions={[5, 10, 25, 50, { label: 'All', value: -1 }]}
                        colSpan={7}
                        count={passes.length}
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

export default AllPasses;