import { useState, useEffect } from 'react';
import {getWorkspaces} from '../Service/WorkspaceApi';
import * as XLSX from 'xlsx';
import { saveAs } from 'file-saver';
import { getDatasWorkspace, getDecision} from '../Service/DataApi';
import {Typography, Button} from '@mui/material';
import {
  ResponsiveContainer,
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Legend,
  ReferenceLine
} from "recharts";
import LocalizedStrings from 'react-localization';
import strings from '../localization.json';

const initialWorkspace = {
  workspaceName: '',
  temperatureThreshold: '',
  humidityThreshold: '',
  gasThreshold: ''
}

const initialDecision = {
  decision: '',
  probability: '',
}

const Dashboard = () => {
  const [selectedWorkspace, setSelectedWorkspace] = useState(initialWorkspace);
  const [decision, setDecision] = useState(initialDecision);
  const [exportData, setExport] = useState(null);
  const [selectedDateFilter, setSelectedDateFilter] = useState('');
  const [data, setData] = useState([]);
  const [workspaces, setWorkspaces] = useState([]);
  const [language] = useState(localStorage.getItem('language') || 'en'); // Default language is English
  const [localization] = useState(()=>{
      return new LocalizedStrings(strings);

  });
  localization.setLanguage(language);
  useEffect(() => {
    fetchWorkspaces();
  }, []);

  const fetchWorkspaces = async () => {
    try {
      const response = await getWorkspaces();
      setWorkspaces(response.data);
    } catch (error) {
      console.error('Error fetching workspaces:', error);
    }
  };

  const formatDate = (data)=>{
    data.map((value)=>(
      value.timestamp = new Intl.DateTimeFormat('en-US', 
      {year: 'numeric', month: '2-digit',day: '2-digit', 
      hour: '2-digit', minute: '2-digit', second: '2-digit'}).format(new Date(value.timestamp)))
    );
  }
  // Function to fetch data based on workspace and date range
  const fetchData = async (workspaceId, date) => {
    try {
      const response = await getDatasWorkspace(workspaceId, date);
      formatDate(response.data);
      setData(response.data);

      const responseDecision = await getDecision(workspaceId);
      if(responseDecision===undefined){
        setDecision({decision:"error",probability: 0})
      }
      setDecision(responseDecision.data);

      const respWorkspace = await getWorkspaces(workspaceId);
      setSelectedWorkspace(respWorkspace.data);
    } catch (error) {
      console.error('Error fetching data:', error);
    }
  };

  const generateExcelFile = (data, selectedWorkspace) => {

    const formattedData = data.map(({ temperature, humidity, gasQuality, id }) => ({
      ID: id,
      Temperature: temperature,
      Humidity: humidity,
      'Gas Quality': gasQuality,
    }));

    const worksheet = XLSX.utils.json_to_sheet(formattedData);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, 'Data');
  
    const workspaceId = selectedWorkspace.id;
    const workspaceName = selectedWorkspace.workspaceName;
    const fileName = `${workspaceId} - ${workspaceName}.xlsx`;
  
    const excelFile = XLSX.write(workbook, { bookType: 'xlsx', type: 'array' });
    const blob = new Blob([excelFile], { type: 'application/octet-stream' });
  
    saveAs(blob, fileName);
  };

  const exportClick =(event) =>{
    generateExcelFile(data, selectedWorkspace);
  }

  const handleWorkspaceChange = (event) => {
    setSelectedWorkspace(event.target.value);
    fetchData(event.target.value, selectedDateFilter);
  };

  const handleDateFilterChange = (event) => {
    setSelectedDateFilter(event.target.value);
    fetchData(selectedWorkspace.id,event.target.value);
  };

  return (
    <div style={{ margin:'15px 0 0 0' }} >
      <Typography style={{ margin:'10px 0 0 20px' }} variant="h4" gutterBottom>{localization.dashboardWorkspace}</Typography>
      <div style={{ margin:'10px 0 0 20px' }}>
        <label>{localization.dashboard_selectWorkspace}: </label>
        <select onChange={handleWorkspaceChange}>
        <option value="">{localization.dashboard_selectWorkspace}</option>
        {workspaces.map((workspace) => (
          <option key={workspace.id} value={workspace.id}>
            {workspace.workspaceName}
          </option> 
        ))}
        </select>
      </div>
      <div style={{ margin:'10px 0 0 20px' }}>
        <label>{localization.dashboard_selectDateFilter}: </label>
        <select value={selectedDateFilter} onChange={handleDateFilterChange}>
          <option value="">{localization.dashboard_noDateFilter}</option>
          <option value="hour">{localization.dashboard_dateLastHour}</option>
          <option value="day">{localization.dashboard_dateToday}</option>
          <option value="week">{localization.dashboard_dateLastWeek}</option>
          <option value="month">{localization.dashboard_dateLastMonth}</option>
        </select>
      </div>
      <Button style={{ margin:'10px 0 0 20px' }} onClick={exportClick} variant="contained">{localization.exportButton}</Button>
      <div style={{ display: 'flex' }}>
        <div style={{ width: '70%', display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center'}}>
        <Typography  variant="h5"  gutterBottom>{localization.gasLabel}</Typography>
          <ResponsiveContainer height={300} width="100%">
            <LineChart data={data} margin={{ right: 25, top: 10 }}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis tickSize={5}  tick={<CustomizedTick />} height={60} dataKey="timestamp" interval="preserveStartEnd"/>
              <YAxis interval="PreserveStart"/>
              <ReferenceLine y={selectedWorkspace.gasThreshold} label="Max" stroke="red" strokeDasharray="4 5" />
              <Line dot={false} type="monotone" dataKey="gasQuality" stroke="#82ca9d" />
            </LineChart>
          </ResponsiveContainer>
        </div>
        <div height={300} style={{ width: '30%', display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center' }}>
          <Typography  variant="h5" gutterBottom>
                    {decision.decision === 'high' ? localization.highDecision :
                    decision.decision === 'medium' ? localization.mediumDecision :
                    decision.decision === 'low' ? localization.lowDecision : localization.noDecision}
          </Typography>
          <Typography
            variant="h6"
            style={{
              color:decision.decision === 'high' ? 'red' :
                    decision.decision === 'medium' ? 'orange' :
                    decision.decision === 'low' ? 'green' : 'purple'
            }}
          >
            {decision.probability}
          </Typography> 
        </div>
      </div>
      <div style={{ display: 'flex' }}>
        <div style={{ width: '50%', display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center' }}>
        <Typography  variant="h5" gutterBottom>{localization.temperatureLabel}</Typography>
          <ResponsiveContainer height={300} width="100%">
            <LineChart data={data} margin={{ right: 15, top: 10 }}>
              <CartesianGrid strokeDasharray="4 4" />
              <XAxis tick={<CustomizedTick />} height={60} dataKey="timestamp" interval="PreserveStart" />
              <YAxis/>
              <ReferenceLine y={selectedWorkspace.temperatureThreshold} label="Max" stroke="red" strokeDasharray="4 5" />
              <Line dot={false} type="monotone" dataKey="temperature" stroke="#82ca9d" />
            </LineChart>       
          </ResponsiveContainer>
        </div>
        <div style={{ width: '50%', display: 'flex', flexDirection: 'column', alignItems: 'center', justifyContent: 'center' }}>
        <Typography  variant="h5" margin={{ left: 25, top: 10 }} gutterBottom>{localization.humidityLabel}</Typography>
          <ResponsiveContainer d height={300} width="100%">
            <LineChart data={data} margin={{ right: 25, top: 10 }}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis tick={<CustomizedTick />} height={60} dataKey="timestamp" interval="PreserveStart" />
              <YAxis interval="PreserveStart" domain={[0, 100]}/>
              <ReferenceLine y={selectedWorkspace.humidityThreshold} label="Max" stroke="red" strokeDasharray="4 5" />
              <Line dot={false} type="monotone" dataKey="humidity" stroke="#82ca9d" />
            </LineChart>
          </ResponsiveContainer>
        </div>
      </div>
    </div>
  );
};

function CustomizedTick(props) {
  const { x, y, stroke, payload } = props;
  const datetime= payload.value.split(", ");
  return (
      <g transform={`translate(${x},${y})`}>
      <text x={0} y={0} dy={16} fill="#666">
        <tspan textAnchor="middle" x="0">
          {datetime[0]}
        </tspan>
        <tspan textAnchor="middle" x="0" dy="20">
        {datetime[1]}
        </tspan>
      </text>
    </g>
  );
}

export default Dashboard;