import {useState} from 'react';
import '../styles/Group.css';
import { useNavigate } from 'react-router-dom';

function Group() {
    const [groupInput, setGroupInput] = useState('');
    const [selectedGroup, setSelectedGroup] = useState('');
    const [studentName, setStudentName] = useState('');
    const [telegramTag, setTelegramTag] = useState('');
    const navigate = useNavigate();

    const handleGroupAdd = () => {
        navigate('/test');
    };

    const handleStudentSave = () => {
        console.log('Сохраняем:', { selectedGroup, studentName, telegramTag });
    };

    const isButtonDisabled = groupInput.trim() === '';

    return (
        <div className="background-wrapper"> {/* Новая обёртка */}
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
                    <option value="ft-201">ФТ-201</option>
                    <option value="ft-202">ФТ-202</option>
                    <option value="ft-203">ФТ-203</option>
                    <option value="ft-204">ФТ-204</option>
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
    );
}


export default Group;