import { useState, useEffect, useCallback } from 'react';
import '../styles/Group.css';
import { useNavigate } from 'react-router-dom';

function Group() {
    const [groupInput, setGroupInput] = useState('');
    const [selectedGroup, setSelectedGroup] = useState('');
    const [selectedGroupId, setSelectedGroupId] = useState(null);
    const [studentName, setStudentName] = useState('');
    const [telegramTag, setTelegramTag] = useState('');
    const [groups, setGroups] = useState([]);
    const navigate = useNavigate();
    const token = localStorage.getItem('token');
    const courseId = localStorage.getItem('courseId');
    const taskId = localStorage.getItem('taskId');


    const fetchGroups = useCallback(async () => {
        try {
            const response = await fetch('http://130.193.59.231:5249/groups/', {
                headers: {
                    'Authorization': `Bearer ${token}`,
                },
            });
            if (!response.ok) {
                throw new Error('Ошибка при получении групп');
            }
            const data = await response.json();
            setGroups(data);
            console.log('Группы:', data);
        } catch (error) {
            console.error('Ошибка:', error);
        }
    }, [token]);

    useEffect(() => {
        if (token) {
            fetchGroups();
        }
    }, [fetchGroups, token]);

    const handleGroupAdd = async () => {
        try {
            const response = await fetch('http://130.193.59.231:5249/groups/', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify({ name: groupInput }),
            });

            if (!response.ok) {
                throw new Error('Ошибка при добавлении группы');
            }

            const newGroup = await response.json();
            setGroups([...groups, newGroup]);
            setGroupInput('');
            fetchGroups();
            console.log('Группа добавлена:', newGroup);
        } catch (error) {
            console.error('Ошибка:', error);
        }
    };

    const handleGroupSelect = (e) => {
        const groupName = e.target.value;
        const group = groups.find(g => g.name === groupName);
        setSelectedGroup(groupName);
        setSelectedGroupId(group ? group.groupId : null);
    };

    const handleStudentSave = async () => {
        if (!selectedGroupId || !studentName.trim() || !telegramTag.trim()) {
            console.error('Не все поля заполнены');
            return;
        }

        try {
            const studentData = {
                name: studentName,
                tgUserName: telegramTag,
                courseId: courseId
            };

            const response = await fetch(`http://130.193.59.231:5249/groups/${selectedGroupId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(studentData),
            });

            if (!response.ok) {
                throw new Error('Ошибка при сохранении студента');
            }

            const savedStudent = await response.json();
            console.log('Студент сохранен:', savedStudent);
            
            // Очищаем поля после успешного сохранения
            setStudentName('');
            setTelegramTag('');
            
            // Можно добавить уведомление об успешном сохранении
            alert('Студент успешно добавлен!');
        } catch (error) {
            console.error('Ошибка:', error);
            alert('Произошла ошибка при сохранении студента');
        }
    };

    const isButtonDisabled = groupInput.trim() === '';

    return (
        <>
            <button
                className="back-button-fixed"
                onClick={() => navigate('/check_homepage')}
            >
                вернуться к проверке дз
            </button>

            <div className="background-wrapper">
                <div className="container">
                    <h2 className="title">добавить группу</h2>
                    <input
                        type="text"
                        value={groupInput}
                        onChange={(e) => setGroupInput(e.target.value)}
                        placeholder="Название группы"
                        className="input-field"
                    />
                    <button
                        onClick={handleGroupAdd}
                        className="button"
                        disabled={isButtonDisabled}
                    >
                        добавить
                    </button>
                </div>

                <div className="container">
                    <h2 className="title">добавить студента</h2>
                    <select
                        className="input-field"
                        value={selectedGroup}
                        onChange={handleGroupSelect}
                    >
                        <option value="">Выберите группу</option>
                        {groups.map((group, index) => (
                            <option key={index} value={group.name}>
                                {group.name}
                            </option>
                        ))}
                    </select>

                    <input
                        type="text"
                        placeholder="Имя Фамилия"
                        value={studentName}
                        onChange={(e) => setStudentName(e.target.value)}
                        className="input-field"
                    />
                    <input
                        type="text"
                        placeholder="@telegram"
                        value={telegramTag}
                        onChange={(e) => setTelegramTag(e.target.value)}
                        className="input-field"
                    />
                    <button
                        onClick={handleStudentSave}
                        className="button"
                        disabled={!selectedGroup || !studentName.trim() || !telegramTag.trim()}
                    >
                        сохранить
                    </button>
                </div>
            </div>
        </>
    );
}

export default Group;