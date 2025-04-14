import { useState, useEffect } from 'react';
import '../styles/Group.css';
import { useNavigate } from 'react-router-dom';

function Group() {
    const [groupInput, setGroupInput] = useState('');
    const [selectedGroup, setSelectedGroup] = useState('');
    const [studentName, setStudentName] = useState('');
    const [telegramTag, setTelegramTag] = useState('');
    const [groups, setGroups] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        const fetchGroups = async () => {
            try {
                const response = await fetch('http://localhost:5249/groups/');
                if (!response.ok) {
                    throw new Error('Ошибка при получении групп');
                }
                const data = await response.json();
                setGroups(data);
                console.log('Группы:', data);
            } catch (error) {
                console.error('Ошибка:', error);
            }
        };

        fetchGroups();
    }, []);

    const handleGroupAdd = async () => {
        try {
            const response = await fetch('http://localhost:5249/groups/', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ name: groupInput }),
            });
    
            if (!response.ok) {
                throw new Error('Ошибка при добавлении группы');
            }
    
            const newGroup = await response.json();
            setGroups([...groups, newGroup]);
            setGroupInput('');
            console.log('Группа добавлена:', newGroup);
        } catch (error) {
            console.error('Ошибка:', error);
        }
    };

    const handleStudentSave = () => {
        console.log('Сохраняем:', { selectedGroup, studentName, telegramTag });
    };

    const isButtonDisabled = groupInput.trim() === '';

    return (
        <>
            <button
                className="back-button-fixed"
                onClick={() => navigate('/test')}
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
                        onChange={(e) => setSelectedGroup(e.target.value)}
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
