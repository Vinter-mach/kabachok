import React, { useState, useEffect, useCallback } from 'react';
import { Viewer, Worker } from '@react-pdf-viewer/core';
import { defaultLayoutPlugin } from '@react-pdf-viewer/default-layout';
import '@react-pdf-viewer/core/lib/styles/index.css';
import '@react-pdf-viewer/default-layout/lib/styles/index.css';
import '../styles/CheckPage.css';

const CheckPage = () => {
    const token = localStorage.getItem('token');
    const courseId = localStorage.getItem('courseId');
    const topicId = localStorage.getItem('taskId');

    const [groups, setGroups] = useState([]);
    const [submissions, setSubmissions] = useState([]);
    const [students, setStudents] = useState([]);
    const [selectedGroupId, setSelectedGroupId] = useState('');
    const [selectedStudent, setSelectedStudent] = useState(null);
    const [comment, setComment] = useState('');
    const [rating, setRating] = useState('');
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const defaultLayoutPluginInstance = defaultLayoutPlugin();

    // Запрос списка групп
    const fetchGroups = useCallback(async () => {
        try {
            const response = await fetch('http://130.193.59.231:5249/groups', {
                headers: { 'Authorization': `Bearer ${token}` }
            });
            if (!response.ok) throw new Error('Ошибка загрузки групп');
            setGroups(await response.json());
        } catch (error) {
            console.error(error);
        }
    }, [token]);

    // Запрос submissions для задания
    const fetchSubmissions = useCallback(async () => {
        try {
            const response = await fetch(
                `http://130.193.59.231:5249/courses/${courseId}/${topicId}/submissions`,
                { headers: { 'Authorization': `Bearer ${token}` } }
            );
            if (!response.ok) throw new Error('Ошибка загрузки работ');
            setSubmissions(await response.json());
        } catch (error) {
            console.error(error);
        }
    }, [courseId, topicId, token]);

    // Запрос студентов группы
    const fetchStudents = useCallback(async (groupId) => {
        try {
            console.log(groupId)
            const response = await fetch(
                `http://130.193.59.231:5249/groups/${groupId}`,
                { headers: { 'Authorization': `Bearer ${token}` } }
            );
            if (!response.ok) throw new Error('Ошибка загрузки студентов');
            setStudents(await response.json());
        } catch (error) {
            console.error(error);
        }
    }, [token]);

    // Загрузка данных при монтировании
    useEffect(() => {
        if (token) {
            fetchGroups();
            fetchSubmissions();
        }
    }, [token, fetchGroups, fetchSubmissions]);

    // Обработчик изменения группы
    useEffect(() => {
        if (selectedGroupId) {
            fetchStudents(selectedGroupId);
        }
    }, [selectedGroupId, fetchStudents]);

    // Получение данных студента из submissions
    const getStudentSubmission = (studentId) => {
        return submissions.find(sub =>
            sub.studentId === studentId && sub.taskId === parseInt(topicId)
        );
    };

    const handleSend = async () => {
        if (!selectedStudent || !selectedStudent.submission) return;

        setLoading(true);
        setError('');

        try {
            const response = await fetch(
                `http://130.193.59.231:5249/courses/${courseId}/${topicId}/submissions/${selectedStudent.submission.submissionId}`,
                {
                    method: 'POST',
                    headers: {
                        'Authorization': `Bearer ${token}`,
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify({
                        grade: Number(rating),
                        comment,
                        statusId: 2,
                        homeworkFile: 'aa'
                    })
                }
            );

            if (!response.ok) {
                throw new Error('Ошибка сохранения оценки');
            }

            // Обновляем локальное состояние
            const updatedSubmissions = submissions.map(sub =>
                sub.submissionId === selectedStudent.submission.submissionId
                    ? {
                        ...sub,
                        grade: Number(rating),
                        comment,
                        statusId: 2
                    }
                    : sub
            );
            setSubmissions(updatedSubmissions);

            setComment('');
            setRating('');
            setSelectedStudent(null);
        } catch (error) {
            console.error('Ошибка:', error);
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };

    const handleStudentSelect = async (student, submission) => {
        if (!submission) return;

        try {
            const response = await fetch(
                `http://130.193.59.231:5249/courses/${courseId}/${topicId}/submissions/${submission.submissionId}`,
                { headers: { 'Authorization': `Bearer ${token}` } }
            );

            if (!response.ok) throw new Error('Ошибка загрузки работы');

            const fullSubmission = await response.json();
            setSelectedStudent({
                ...student,
                submission: fullSubmission[0]
            });
        } catch (error) {
            console.error('Ошибка при загрузке работы:', error);
            setError('Не удалось загрузить работу студента');
        }
    };


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
                    min="0"
                    max="100"
                />
                {error && <div className="error-message">{error}</div>}
                <button
                    onClick={handleSend}
                    disabled={loading}
                >
                    {loading ? 'Отправка...' : 'Отправить'}
                </button>
            </div>

            <div className="center-panel">
                {selectedStudent ? (
                    <Worker workerUrl="https://unpkg.com/pdfjs-dist@3.9.179/build/pdf.worker.min.js">
                        <Viewer
                            fileUrl={selectedStudent.submission.homeworkFile}  // Динамический URL
                            plugins={[defaultLayoutPluginInstance]}
                        />
                    </Worker>
                ) : (
                    <div className="empty-panel">Выберите студента</div>
                )}
            </div>

            <div className="left-panel">
                <select
                    value={selectedGroupId}
                    onChange={e => setSelectedGroupId(e.target.value)}
                >
                    <option value="">Выберите группу</option>
                    {groups.map(group => (
                        <option key={group.groupId} value={group.groupId}>
                            {group.name}
                        </option>
                    ))}
                </select>

                <table>
                    <tbody>
                        {students.map(student => {
                            const submission = getStudentSubmission(student.studentId);
                            return (
                                <tr
                                    key={student.studentId}
                                    className={selectedStudent?.studentId === student.studentId ? 'selected' : ''}
                                >
                                    <td>{submission?.grade || '—'}</td>
                                    <td>
                                        <button onClick={() => handleStudentSelect(student, submission)}>
                                            {student.name}
                                        </button>
                                    </td>
                                </tr>
                            );
                        })}
                    </tbody>
                </table>
            </div>
        </div>
    );
};

export default CheckPage;