import { useState, useEffect } from 'react';
import React from 'react';
import { Table, TableHead, TableCell, TableRow, TableBody, Button, styled, Container, TablePagination,TableFooter } from '@mui/material'
import { getWorkspaces, deleteWorkspace } from '../../Service/WorkspaceApi';
import { Link } from 'react-router-dom';
import LocalizedStrings from 'react-localization';
import strings from '../../localization.json'
const StyledTable = styled(Table)`
    width: 90%;
    margin: 20px 0 0 50px;
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


const AllWorkspaces = () => {
    const [workspaces, setWorkspaces] = useState([]);
    const [page, setPage] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(5);
    const [language] = useState(localStorage.getItem('language') || 'en'); // Default language is English
    const [localization] = useState(()=>{
          return new LocalizedStrings(strings);
  
    });
    localization.setLanguage(language);

    useEffect(() => {
        getAllWorkspaces();
    }, []);

    const deleteWorkspaceData = async (id) => {
        await deleteWorkspace(id);
        getAllWorkspaces();
    }
    const handleChangePage = (event, newPage) => {
        setPage(newPage);
      };
    
      const handleChangeRowsPerPage = (event) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
      };
    const getAllWorkspaces = async () => {
        let response = await getWorkspaces();
        if(response !==undefined)
            setWorkspaces(response.data);
    }

    return (
        <div>
            <TContainer maxWidth="1200">
                <StyledButton variant = "contained" href="workspaces/add">{localization.addWorkspaceLabel}</StyledButton>
            </TContainer>
        <StyledTable>
            
            <TableHead>
                <THead>
                    <TableCell>Id</TableCell>
                    <TableCell>{localization.workspaceNameLabel}</TableCell>
                    <TableCell>{localization.temperatureThresholdLabel}</TableCell>
                    <TableCell>{localization.humidityThresholdLabel}</TableCell>
                    <TableCell>{localization.gasThresholdLabel}</TableCell>
                    <TableCell></TableCell>
                </THead>
            </TableHead>
            <TableBody>
                {(rowsPerPage > 0
                ? workspaces.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                : workspaces
            ).map((workspace) => (
                    <TRow key={workspace.id}>
                        <TableCell>{workspace.id}</TableCell>
                        <TableCell>{workspace.workspaceName}</TableCell>
                        <TableCell>{workspace.temperatureThreshold}</TableCell>
                        <TableCell>{workspace.humidityThreshold}</TableCell>
                        <TableCell>{workspace.gasThreshold}</TableCell>
                        <TableCell>
                            <Button color="primary" variant="contained" style={{marginRight:10}} component={Link} to={`edit/${workspace.id}`}>{localization.editLabel}</Button>
                            <Button color="secondary" variant="contained" onClick={() => deleteWorkspaceData(workspace.id)}>{localization.deleteLabel}</Button> 
                        </TableCell>
                    </TRow>
                ))}
            </TableBody>
            <TableFooter>
                <TableRow>
                    <TablePagination
                        rowsPerPageOptions={[5, 10, 25, 50, { label: 'All', value: -1 }]}
                        colSpan={6}
                        count={workspaces.length}
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

export default AllWorkspaces;