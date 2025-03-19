import { useState, useEffect } from "react";
import {
  LineChart,
  Line,
  XAxis,
  YAxis,
  CartesianGrid,
  Tooltip,
  ResponsiveContainer,
} from "recharts";
import {
  getViewAnalytic,
  getMonthViewAnalytic,
} from "../services/pasteService";

const ViewAnalytic = () => {
  const [allMonthlyData, setAllMonthlyData] = useState([]);
  const [monthlyViewData, setMonthlyViewData] = useState([]);
  const [selectedMonth, setSelectedMonth] = useState("");
  const [monthsToShow, setMonthsToShow] = useState(6);
  const [allDailyData, setAllDailyData] = useState({});
  const [dailyViewData, setDailyViewData] = useState([]);

  useEffect(() => {
    const fetchMonthlyData = async () => {
      try {
        const data = await getViewAnalytic();
        const formattedData = data
          .split("|")
          .map((entry) => {
            const [time, views] = entry.split(";");
            return {
              date: time.replace("time=", ""),
              views: parseInt(views.replace("views=", "")),
            };
          })
          .sort((a, b) => new Date(a.date) - new Date(b.date));
        console.log(formattedData)
        setAllMonthlyData(formattedData);
        setMonthlyViewData(formattedData.slice(-monthsToShow));
        if (formattedData.length > 0) {
          setSelectedMonth(formattedData[formattedData.length - 1].date);
        }
      } catch (error) {
        console.error("Error fetching monthly view data:", error);
      }
    };
    fetchMonthlyData();
  }, []);

  useEffect(() => {
    if (allMonthlyData.length > 0) {
      setMonthlyViewData(allMonthlyData.slice(-monthsToShow)); 
    }
  }, [monthsToShow, allMonthlyData]);
  useEffect(() => {
    if (allMonthlyData.length > 0) {
      setMonthsToShow((prev) => Math.min(prev, allMonthlyData.length));
      setMonthlyViewData(allMonthlyData.slice(-monthsToShow));
    }
  }, [monthsToShow, allMonthlyData]);
  useEffect(() => {
    const fetchAllDailyData = async () => {
      try {
        let dailyData = {};
        for (const month of allMonthlyData) {
          const data = await getMonthViewAnalytic(month.date);
          console.log(data)
          dailyData[month.date] = data
            .split("|")
            .map((entry) => {
              const [time, views] = entry.split(";");
              return {
                date: time.replace("time=", ""),
                views: parseInt(views.replace("views=", "")),
              };
            })
            .sort((a, b) => a.date.localeCompare(b.date)); 
        }
        setAllDailyData(dailyData);
        if (selectedMonth) {
          setDailyViewData(dailyData[selectedMonth] || []);
        }
      } catch (error) {
        console.error("Error fetching daily view data:", error);
      }
    };
    if (allMonthlyData.length > 0) {
      fetchAllDailyData();
    }
  }, [allMonthlyData]);

  useEffect(() => {
    if (selectedMonth && allDailyData[selectedMonth]) {
      setDailyViewData(allDailyData[selectedMonth]);
    }
  }, [selectedMonth, allDailyData]);

  return (
    <div
      style={{
        width: "95vw",
        height: "200vh",
        padding: "20px",
        backgroundColor: "#f4f4f4",
      }}
    >
      <div style={{ marginBottom: "20px", textAlign: "center" }}>
        <label>Months to Show: </label>
        <input
          type="number"
          value={monthsToShow}
          onChange={(e) =>
            setMonthsToShow(Math.max(1, parseInt(e.target.value)))
          }
          style={{ width: "60px", marginRight: "20px" }}
        />
      </div>

      <div
        style={{
          width: "90%",
          height: "40%",
          backgroundColor: "#fff",
          padding: "20px",
          borderRadius: "8px",
          marginBottom: "20px",
        }}
      >
        <h3 style={{ textAlign: "center" }}>📈 Monthly Views</h3>
        <ResponsiveContainer width="100%" height="80%">
          <LineChart data={monthlyViewData}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="date" />
            <YAxis />
            <Tooltip />
            <Line
              type="monotone"
              dataKey="views"
              stroke="#8884d8"
              strokeWidth={2}
            />
          </LineChart>
        </ResponsiveContainer>
      </div>

      <div
        style={{
          marginTop: "200px",
          marginBottom: "20px",
          textAlign: "center",
        }}
      >
        <label>Select Month: </label>
        <select
          value={selectedMonth}
          onChange={(e) => setSelectedMonth(e.target.value)}
        >
          {allMonthlyData.map((month) => (
            <option key={month.date} value={month.date}>
              {month.date}
            </option>
          ))}
        </select>
      </div>

      <div
        style={{
          width: "90%",
          height: "40%",
          backgroundColor: "#fff",
          padding: "20px",
          borderRadius: "8px",
        }}
      >
        <h3 style={{ textAlign: "center" }}>
          📅 Daily Views for {selectedMonth}
        </h3>
        <ResponsiveContainer width="100%" height="80%">
          <LineChart data={dailyViewData}>
            <CartesianGrid strokeDasharray="3 3" />
            <XAxis dataKey="date" />
            <YAxis />
            <Tooltip />
            <Line
              type="monotone"
              dataKey="views"
              stroke="#82ca9d"
              strokeWidth={2}
            />
          </LineChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
};

export default ViewAnalytic;
