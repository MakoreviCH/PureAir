import { AppBar, Button, Container, Toolbar, styled } from '@mui/material';
import { useState } from 'react';
import { FormControl, Radio, RadioGroup, FormControlLabel } from '@mui/material';
import { NavLink, useNavigate } from 'react-router-dom';
import AuthService from '../Service/AuthService';
import LocalizedStrings from 'react-localization';
import strings from '../localization.json';

const authService = new AuthService();
const Header = styled(AppBar)`
    background: #111111;
`;
    
const Tabs = styled(NavLink)`
    color: #FFFFFF;
    margin-right: 20px;
    text-decoration: none;
    font-size: 20px;
`;
const TButton = styled(Button)`
    color: #FFFFFF;
    font-size: 20px;
    `;
const RightContainer = styled(Container)`
    justify-content: flex-end;
    display: flex;
    max-width:100% !important;
    margin: 0 0 0 0 !important;
    padding:0 !important;
    align-items: flex-end !important;
`;
const NavBar = () => {
    const navigate = useNavigate();

    const [language, setLang] = useState(localStorage.getItem('language') || 'en'); // Default language is English
    const [localization] = useState(()=>{
        return new LocalizedStrings(strings);

    });
    localization.setLanguage(language);

    const handleLogout = () => {
        authService.logout();
        navigate('/login');
    };

    const handleChangeLanguage = (selectedLanguage) => {
        setLang(selectedLanguage.target.value);
        localization.setLanguage(selectedLanguage.target.value);
        localStorage.setItem('language', selectedLanguage.target.value);
        window.location.reload(false);     
    };
    return (
        <Header position="static">
            <Toolbar>
                <Tabs to="./" exact>{localization.dashboardTitle}</Tabs>
                <Tabs to="users" exact>{localization.usersTitle}</Tabs>
                <Tabs to="passes" exact>{localization.passesTitle}</Tabs>
                <Tabs to="workspaces" exact>{localization.workspacesTitle}</Tabs>
                <Tabs to="datas" exact>{localization.datasTitle}</Tabs>
                <RightContainer>
                    <FormControl>
                        <RadioGroup value={language} onChange={handleChangeLanguage}>
                            <FormControlLabel value="en" control={<Radio />} labelPlacement="" label="EN" />
                            <FormControlLabel value="uk" control={<Radio />} label="UK" />
                        </RadioGroup>
                    </FormControl>
                    <TButton onClick={handleLogout}>{localization.logout}</TButton>
                </RightContainer>
            </Toolbar>
           
            
        </Header>
    )
}

export default NavBar;