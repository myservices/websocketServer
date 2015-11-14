package testtask.websocket.server.endpoints;

import javax.websocket.*;
import javax.websocket.server.ServerEndpoint;
import java.io.IOException;
import java.util.Timer;
import java.util.TimerTask;
import java.util.UUID;
import java.util.logging.Logger;

/**
 * Created on 14.11.2015.
 */
@ServerEndpoint(value = "/messenger")
public class Messenger {
    private long sendPeriod=20000;
    private Logger logger = Logger.getLogger(this.getClass().getName());
    Timer timer=new Timer();

    private class SendRandomStringTask extends TimerTask {
        private Session session;

        protected SendRandomStringTask(Session session) {
            super();
            this.session=session;
        }

        @Override
        public void run() {
            try {
                if(session.isOpen()) session.getBasicRemote().sendText(UUID.randomUUID().toString());
                else this.cancel();
            } catch (IOException e) {
                logger.warning("Failed to send text:"+e);
                this.cancel();
            } catch (Exception e){
                logger.warning("Something went wrong:"+e);
            }
        }
    }

    @OnOpen
    public void onOpen(Session session) {
        logger.info("Connected ... " + session.getId());
        timer.scheduleAtFixedRate(new SendRandomStringTask(session),sendPeriod,sendPeriod);
    }

    @OnMessage
    public String onMessage(String message, Session session) {
        logger.info(message);
        switch (message) {
            case "quit":
                try {
                    session.close(new CloseReason(CloseReason.CloseCodes.NORMAL_CLOSURE, "Game ended"));
                } catch (IOException e) {
                    throw new RuntimeException(e);
                }
                break;
        }
        return UUID.randomUUID().toString();
    }

    @OnClose
    public void onClose(Session session, CloseReason closeReason) {
        logger.info(String.format("Session %s closed because of %s", session.getId(), closeReason));

    }
}
