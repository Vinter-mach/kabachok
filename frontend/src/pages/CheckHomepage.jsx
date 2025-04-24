import { useState, useEffect, useCallback } from 'react';
import '../styles/CheckHomepage.css';
import { useNavigate } from 'react-router-dom';

function CheckHomepage() {
    const [topicInput, setTopicInput] = useState('');
    const [selectedTopic, setSelectedTopic] = useState('');
    const [topicName, setTopicName] = useState('');
    const [deadline, setDeadline] = useState('');
    const [topics, setTopics] = useState([]);
    const navigate = useNavigate();
    const token = localStorage.getItem('token');
    const courseId = localStorage.getItem('courseId');

    const fetchTopics = useCallback(async () => {
        try {
            const response = await fetch(`http://localhost:5249/courses/${courseId}/`, {
                headers: {
                    'Authorization': `Bearer ${token}`,
                },
            });
            if (!response.ok) {
                throw new Error('Ошибка при получении тем');
            }
            const data = await response.json();
            setTopics(data);
            console.log('Темы:', data);
        } catch (error) {
            console.error('Ошибка:', error);
        }
    }, [token]);

    useEffect(() => {
        if (token) {
          fetchTopics();
        }
      }, [fetchTopics, token]);

    const handleTopicAdd = async () => {
        try {
            const response = await fetch(`http://localhost:5249/courses/${courseId}/`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify({ name: topicInput }),
            });

            if (!response.ok) {
                throw new Error('Ошибка при добавлении темы');
            }

            const newTopic = await response.json();
            setTopics([...topics, newTopic]);
            setTopicInput('');
            fetchTopics();
            console.log('Тема добавлена:', newTopic);
        } catch (error) {
            console.error('Ошибка:', error);
        }
    };

    const handleTopicSave = async () => {
        if (!topicName.trim() || !deadline.trim()) {
            console.error('Не все поля заполнены');
            return;
        }

        try {
            const topicData = {
                taskId: 0, // обычно сервер сам генерирует ID
                name: topicName,
                taskLink: 'string', // я не понял что это
                deadline: '2024-05-01T14:30:00Z',
                teacherId: 1, // хз откуда это брать тоже
                isGraves: false // пока что так
            };

            const response = await fetch(`http://localhost:5249/courses/${courseId}`, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}`
                },
                body: JSON.stringify(topicData),
            });

            if (!response.ok) {
                throw new Error('Ошибка при сохранении темы');
            }

            const savedTopic = await response.json();
            console.log('Тема сохранена:', savedTopic);
            
            // Очищаем поля после успешного сохранения
            setTopicName('');
            setDeadline('');
            
            // Можно добавить уведомление об успешном сохранении
            alert('Тема успешно добавлена!');
        } catch (error) {
            console.error('Ошибка:', error);
            alert('Произошла ошибка при сохранении темы');
        }
    };

    const isButtonDisabled = !selectedTopic;

    const handleNavigate = () => {
        if (selectedTopic) {
            const topic = topics.find(topic => topic.name === selectedTopic);
            localStorage.setItem('taskId', topic.taskId);
            navigate('/check', { state: { topicId: topic.taskId } });
        }
    };

    return (
        <>
            <button
                className="back-button-fixed"
                onClick={() => navigate('/group')}
            >
                добавить студента или группу
            </button>

            <div className="background-wrapper">
                <div className="container">
                    <h2 className="title">выбрать тему для проверки</h2>
                    <select
                        className="input-field"
                        value={selectedTopic}
                        onChange={(e) => setSelectedTopic(e.target.value)}
                    >
                        <option value="">Выберите тему</option>
                        {topics.map((topic, index) => (
                            <option key={index} value={topic.name}>
                                {topic.name}
                            </option>
                        ))}
                    </select>
                    <button
                        onClick={handleNavigate}
                        className="button"
                        disabled={isButtonDisabled}
                    >
                        проверять
                    </button>
                </div>

                <div className="container">
                    <h2 className="title">добавить тему</h2>
                    <input
                        type="text"
                        placeholder="Название темы"
                        value={topicName}
                        onChange={(e) => setTopicName(e.target.value)}
                        className="input-field"
                    />
                    <input
                        type="text"
                        placeholder="Дедлайн"
                        value={deadline}
                        onChange={(e) => setDeadline(e.target.value)}
                        className="input-field"
                    />
                    <button
                        onClick={handleTopicSave}
                        className="button"
                        disabled={!topicName.trim() || !deadline.trim()}
                    >
                        добавить
                    </button>
                </div>
            </div>
        </>
    );
}

export default CheckHomepage;
