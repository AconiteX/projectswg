package com.swgproject.projectswg.library;

import java.net.*;

public class CheckStatus {
	//Check if the server is online
	
	public static boolean check() {

		try {
			byte[] receiveData = new byte[1024];
			InetAddress address = InetAddress.getByName("23.29.118.50");
			//create socket
			DatagramSocket clientSocket = new DatagramSocket();
			//set timeout
			clientSocket.setSoTimeout(1000);
			//send packet
			DatagramPacket p = new DatagramPacket(Integer.toBinaryString(0x0006000000).getBytes(), 5, address, 44462);
			clientSocket.send(p);
			//try to receive packet from server
			DatagramPacket receivePacket = new DatagramPacket(receiveData, receiveData.length);
			clientSocket.receive(receivePacket);
			clientSocket.close();
			return true;
 		} catch (Exception e) {
 			//if nothing is received set the the server status offline
 			return false;
		}
	}
	
}
