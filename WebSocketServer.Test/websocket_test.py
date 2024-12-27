import asyncio
import websockets
import pytest

@pytest.mark.asyncio
async def test_websocket_connection():
    uri = "ws://localhost:5000/"
    async with websockets.connect(uri) as websocket:
        await websocket.send("Hello, WebSocket!")
        response = await websocket.recv()
        assert response == "Echo: Hello, WebSocket!"

@pytest.mark.asyncio
async def test_websocket_multiple_messages():
    uri = "ws://localhost:5000/"
    async with websockets.connect(uri) as websocket:
        messages = ["Message 1", "Message 2", "Message 3"]
        for message in messages:
            await websocket.send(message)
            response = await websocket.recv()
            assert response == f"Echo: {message}"

@pytest.mark.asyncio
async def test_websocket_close():
    uri = "ws://localhost:5000/"
    async with websockets.connect(uri) as websocket:
        await websocket.send("Close")
        response = await websocket.recv()
        assert response == "Echo: Close"
        await websocket.close()