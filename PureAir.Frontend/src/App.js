import AllUsers from './Component/Get/AllUsers';
import AddUser from './Component/Add/AddUser';
import EditUser from './Component/Edit/EditUser';
import AllWorkspaces from './Component/Get/AllWorkspaces';
import NavBar from './Component/NavBar';
import NotFound from './Component/NotFound'; 
import { BrowserRouter, Route, Routes } from 'react-router-dom';
import AddWorkspace from './Component/Add/AddWorkspace';
import EditWorkspace from './Component/Edit/EditWorkspace';
import AllPasses from './Component/Get/AllPasses';
import AllDatas from './Component/Get/AllDatas';
import AddPass from './Component/Add/AddPass';
import Dashboard from './Component/Dashboard';
import LoginPage from './Component/Login';
import ProtectedRoute from './Component/ProtectedRoute';
import EditPass from './Component/Edit/EditPass';


function App() {
  return (
    <BrowserRouter>
      <NavBar />
      <Routes>
        <Route exact path="/login" element={<LoginPage/>} />
        <Route exact path='/' element={<ProtectedRoute/>}>
            <Route exact path='/' element={<Dashboard/>}/>
        </Route>
        <Route exact path='/' element={<ProtectedRoute/>}>
          <Route  path="/users" element={<AllUsers />} />
        </Route>
        <Route exact path='/' element={<ProtectedRoute/>}>
          <Route  path="/users/add" element={<AddUser />} />
        </Route>
        <Route exact path='/' element={<ProtectedRoute/>}>
          <Route  path="/users/edit/:id" element={<EditUser />} />
        </Route>
        <Route exact path='/' element={<ProtectedRoute/>}>
          <Route  path="/workspaces" element={<AllWorkspaces/>} />
        </Route>
        <Route exact path='/' element={<ProtectedRoute/>}>
          <Route  path="/workspaces/add" element={<AddWorkspace/>} />
        </Route>
        <Route exact path='/' element={<ProtectedRoute/>}>
          <Route  path="/workspaces/edit/:id" element={<EditWorkspace/>} />
        </Route>
        <Route exact path='/' element={<ProtectedRoute/>}>
          <Route  path="/passes" element={<AllPasses/>} />
        </Route>
        <Route exact path='/' element={<ProtectedRoute/>}>
          <Route  path="/passes/add" element={<AddPass />} />
        </Route>
        <Route exact path='/' element={<ProtectedRoute/>}>
          <Route  path="/passes/edit/:id" element={<EditPass/>} />
        </Route>
        <Route exact path='/' element={<ProtectedRoute/>}>
          <Route  path="/datas" element={<AllDatas/>} />
        </Route>
        <Route element={<NotFound />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
