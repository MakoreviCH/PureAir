import { useState, useEffect } from 'react';
import React from 'react';
import { Table, TableHead, TableCell, TableRow, TableBody, Button, styled, TablePagination,TableFooter } from '@mui/material'
import { getDatas, deleteData } from '../../Service/DataApi';
import LocalizedStrings from 'react-localization';
import strings from '../../localization.json'
const StyledTable = styled(Table)`
    width: 90%;
    margin: 20px 0 0px 50px;
    
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


const AllDatas = () => {
    const [page, setPage] = React.useState(0);
    const [rowsPerPage, setRowsPerPage] = React.useState(5);
    const [datas, setDatas] = useState([]);
    const [language] = useState(localStorage.getItem('language') || 'en'); // Default language is English
    const [localization] = useState(()=>{
          return new LocalizedStrings(strings);
    });
    localization.setLanguage(language);
    useEffect(() => {
        getAllDatas();
    }, []);

    const handleChangePage = (event, newPage) => {
        setPage(newPage);
      };
    
      const handleChangeRowsPerPage = (event) => {
        setRowsPerPage(parseInt(event.target.value, 10));
        setPage(0);
      };
    const deleteWorkspaceDatas = async (id) => {
        await deleteData(id);
        getAllDatas();
    }

    const getAllDatas = async () => {
        let response = await getDatas();
        if(response !==undefined)
            setDatas(response.data);
    }

    return (

        <div>
        
        <StyledTable>
            
            <TableHead>
                <THead>
                    <TableCell>{localization.dataLabel}</TableCell>
                    <TableCell>{localization.workspaceOnly}</TableCell>
                    <TableCell>{localization.temperatureLabel}</TableCell>
                    <TableCell>{localization.humidityLabel}</TableCell>
                    <TableCell>{localization.gasLabel}</TableCell>
                    <TableCell>{localization.timestamp}</TableCell>
                    <TableCell></TableCell>
                </THead>
            </TableHead>
            <TableBody>
                    {(rowsPerPage > 0
                ? datas.slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                : datas
            ).map((data) => (
                    <TRow key={data.id}>
                        <TableCell>{data.id}</TableCell>
                        <TableCell>{data.workspaceName}</TableCell>
                        <TableCell>{data.temperature}</TableCell>
                        <TableCell>{data.humidity}</TableCell>
                        <TableCell>{data.gasQuality}</TableCell>
                        <TableCell>{data.timestamp}</TableCell>
                        <TableCell>
                            <Button color="secondary" variant="contained" onClick={() => deleteWorkspaceDatas(data.id)}>{localization.deleteLabel}</Button> 
                        </TableCell>
                    </TRow>
                ))}
            </TableBody>
            <TableFooter>
                <TableRow>
                    <TablePagination
                        rowsPerPageOptions={[5, 10, 25, 50, { label: 'All', value: -1 }]}
                        colSpan={7}
                        count={datas.length}
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

export default AllDatas;