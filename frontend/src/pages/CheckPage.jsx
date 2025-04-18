import React, { useState, useEffect } from 'react';
import { Viewer, Worker } from '@react-pdf-viewer/core';
import { defaultLayoutPlugin } from '@react-pdf-viewer/default-layout';
import '@react-pdf-viewer/core/lib/styles/index.css';
import '@react-pdf-viewer/default-layout/lib/styles/index.css';
import '../styles/CheckPage.css';

const topicId = localStorage.getItem('topicId');

const initialGroupData = {
    'Group A': [
        { name: 'A', value: null, comment: null, pdfFile: '/example.pdf' },
        { name: 'B', value: null, comment: null, pdfFile: '/example2.pdf' },
    ],
    'Group B': [
        { name: 'B', value: null, comment: null, pdfFile: '/example.pdf' },
    ],
};

const getPdfFile = (group, name) => {
    const list = initialGroupData[group] || [];
    const item = list.find(i => i.name === name);
    return item ? item.pdfFile : '';
};

const CheckPage = () => {
    const defaultLayoutPluginInstance = defaultLayoutPlugin();

    const [groupData, setGroupData] = useState(() => {
        const saved = localStorage.getItem('groupData');
        return saved ? JSON.parse(saved) : initialGroupData;
    });

    useEffect(() => {
        localStorage.setItem('groupData', JSON.stringify(groupData));
    }, [groupData]);

    const [selectedGroup, setSelectedGroup] = useState(Object.keys(initialGroupData)[0]);
    const [selectedName, setSelectedName]   = useState('');
    const [comment, setComment]             = useState('');
    const [rating, setRating]               = useState('');

    const handleSend = () => {
        if (!selectedName) return;

        setGroupData(prev => ({
            ...prev,
            [selectedGroup]: prev[selectedGroup].map(item =>
                item.name === selectedName
                    ? { ...item, value: Number(rating), comment }
                    : item
            ),
        }));

        setComment('');
        setRating('');
        setSelectedName('');
    };

    const pdfUrl = getPdfFile(selectedGroup, selectedName);

    return (
        <div className="page-container">
            <div className="right-panel">
                <textarea
                    placeholder="Комментарий"
                    value={comment}
                    onChange={e => setComment(e.target.value)}
                />
                <input
                    type="number"
                    placeholder="Оценка"
                    value={rating}
                    onChange={e => setRating(e.target.value)}
                />
                <button onClick={handleSend}>Отправить</button>
            </div>

            <div className="center-panel">
                {pdfUrl ? (
                    <Worker workerUrl="https://unpkg.com/pdfjs-dist@3.9.179/build/pdf.worker.min.js">
                        <Viewer fileUrl={pdfUrl} plugins={[defaultLayoutPluginInstance]} />
                    </Worker>
                ) : (
                    <div className="empty-panel">Выберите имя с PDF</div>
                )}
            </div>

            <div className="left-panel">
                <select
                    value={selectedGroup}
                    onChange={e => {
                        setSelectedGroup(e.target.value);
                        setSelectedName('');
                    }}
                >
                    {Object.keys(groupData).map(group => (
                        <option key={group} value={group}>
                            {group}
                        </option>
                    ))}
                </select>


                <table>
                    <tbody>
                    {groupData[selectedGroup].map(item => (
                        <tr
                            key={item.name}
                            className={selectedName === item.name ? 'selected' : ''}
                        >
                            <td>{item.value}</td>
                            <td>
                                <button onClick={() => setSelectedName(item.name)}>
                                    {item.name}
                                </button>
                            </td>
                        </tr>
                    ))}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default CheckPage;
