import React from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import Authorization from "./pages/Authorization.jsx";
import Course from "./pages/Course.jsx";
import Group from "./pages/Group.jsx";
import CheckPage from "./pages/CheckPage.jsx";

function App() {
    return (
        <BrowserRouter>
            <Routes>
                <Route path="/" element={<Authorization />} />
                <Route path="/courses" element={<Course />} />
                <Route path="/group" element={<Group />} />
                <Route path="/test" element={<CheckPage />} />
            </Routes>
        </BrowserRouter>
    );
}

export default App;
