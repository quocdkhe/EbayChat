const chats = [
    {
        id: 1,
        name: "John Anderson",
        preview: "Thank you! 😊",
        online: true,
        avatar: "https://i.pravatar.cc/150?img=1",
        messages: [
            { type: "received", text: "Hi! How are you?", time: "10:30 AM", date: "Yesterday" },
            { type: "sent", text: "Great! How about you?", time: "10:31 AM", date: "Yesterday" },
            { type: "received", text: "I'm good. Are you busy today?", time: "10:32 AM", date: "Yesterday" },
            { type: "sent", text: "No, I'm free. What do you need?", time: "10:33 AM", date: "Yesterday" },
            { type: "received", text: "Thank you! 😊", time: "10:34 AM", date: "Yesterday" }
        ]
    },
    {
        id: 2,
        name: "Sarah Wilson",
        preview: "OK, all done!",
        online: true,
        avatar: "https://i.pravatar.cc/150?img=2",
        messages: [
            { type: "received", text: "How was the morning meeting?", time: "09:00 AM", date: "Today" },
            { type: "sent", text: "Very good, achieved great results", time: "09:05 AM", date: "Today" },
            { type: "received", text: "Awesome! Do you have the files?", time: "09:10 AM", date: "Today" },
            { type: "sent", text: "Yes, I'll send them later", time: "09:11 AM", date: "Today" },
            { type: "received", text: "OK, all done!", time: "09:12 AM", date: "Today" }
        ]
    },
    {
        id: 3,
        name: "Michael Chen",
        preview: "I'll do it right now",
        online: false,
        avatar: "https://i.pravatar.cc/150?img=3",
        messages: [
            { type: "received", text: "Can you help me with something?", time: "08:45 AM", date: "2 days ago" },
            { type: "sent", text: "Sure, I can help. What do you need?", time: "08:50 AM", date: "2 days ago" },
            { type: "received", text: "I'll do it right now", time: "08:52 AM", date: "2 days ago" }
        ]
    },
    {
        id: 4,
        name: "Emma Martinez",
        preview: "Oh, I forgot about that...",
        online: true,
        avatar: "https://i.pravatar.cc/150?img=4",
        messages: [
            { type: "sent", text: "How are you doing?", time: "07:30 AM", date: "3 days ago" },
            { type: "received", text: "Great! How about you?", time: "07:35 AM", date: "3 days ago" },
            { type: "received", text: "Oh, I forgot about that...", time: "07:40 AM", date: "3 days ago" }
        ]
    }
];

let currentChat = null;

// Initialize chat list
function initChatList() {
    const chatList = document.getElementById('chatList');
    chatList.innerHTML = '';

    chats.forEach(chat => {
        const chatItem = document.createElement('div');
        chatItem.className = 'chat-item';
        chatItem.innerHTML = `
                    <img src="${chat.avatar}" alt="${chat.name}" class="chat-avatar">
                    <div class="chat-info">
                        <div class="chat-name">${chat.name}</div>
                        <div class="chat-preview">${chat.preview}</div>
                    </div>
                `;
        chatItem.addEventListener('click', () => selectChat(chat.id));
        chatList.appendChild(chatItem);
    });
}

// Select chat
function selectChat(chatId) {
    currentChat = chats.find(c => c.id === chatId);

    // Update sidebar active
    document.querySelectorAll('.chat-item').forEach((item, index) => {
        item.classList.toggle('active', chats[index].id === chatId);
    });

    // Update header
    document.getElementById('chatName').textContent = currentChat.name;
    document.getElementById('chatStatus').textContent = currentChat.online ? 'Active now' : 'Offline';

    // Load messages
    displayMessages();
}

// Render messages function
function displayMessages() {
    const messagesContainer = document.getElementById('messagesContainer');
    messagesContainer.innerHTML = '';

    currentChat.messages.forEach(msg => {
        const messageDiv = document.createElement('div');
        messageDiv.className = `message ${msg.type}`;
        messageDiv.innerHTML = `
                    <div class="message-bubble">
                        <div>${msg.text}</div>
                        <div class="message-time">${msg.time} | ${msg.date}</div>
                    </div>
                `;
        messagesContainer.appendChild(messageDiv);
    });
    messagesContainer.scrollTop = messagesContainer.scrollHeight;
}

// Send message
function sendMessage() {
    const input = document.getElementById('messageInput');
    const text = input.value.trim();

    if (!text || !currentChat) return;

    const now = new Date();
    const time = now.getHours().toString().padStart(2, '0') + ':' +
        now.getMinutes().toString().padStart(2, '0');
    const hours = now.getHours();
    const ampm = hours >= 12 ? 'PM' : 'AM';
    const displayTime = ((hours % 12) || 12) + ':' + now.getMinutes().toString().padStart(2, '0') + ' ' + ampm;
    const date = 'Today';

    currentChat.messages.push({
        type: 'sent',
        text: text,
        time: displayTime,
        date: date
    });

    input.value = '';
    displayMessages();
}

// Event listeners
document.getElementById('sendBtn').addEventListener('click', sendMessage);
document.getElementById('messageInput').addEventListener('keypress', (e) => {
    if (e.key === 'Enter') sendMessage();
});

// Initialize
initChatList();
if (chats.length > 0) {
    selectChat(chats[0].id);
}