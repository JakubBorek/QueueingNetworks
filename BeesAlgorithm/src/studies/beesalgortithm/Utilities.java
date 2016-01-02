package studies.beesalgortithm;

import java.util.Collections;
import java.util.Vector;

public class Utilities {
	
	public static Vector<Scout> createScouts(int scoutsAmount){
		Vector<Scout> scouts = new Vector<Scout>(); 
		for(int i=0;i<scoutsAmount;i++){
			scouts.add(new Scout()); 
		}
		return scouts; 
	}
	
	public static void sendScouts(Vector<Scout> scouts) {
		for(Scout scout:scouts){
			scout.choseFlower(); 
		}
	}

	public static void searchEliteFlowersNeigberhood(Vector<Scout> scoutsWithEliteFlowers) {
		Flower bestNeighbor; 
		for(Scout scout : scoutsWithEliteFlowers){
			bestNeighbor = chooseBestNeighbor(scout.getFlower(), BeesAlgorithm.getElisteSolutionsAmount());
			scout = new Scout(bestNeighbor);
		}
	}


	public static void searchBestFlowersNeigberhood(Vector<Scout> scoutsWithBestFlowers) {
		Flower bestNeighbor; 
		for(Scout scout : scoutsWithBestFlowers){
			bestNeighbor = chooseBestNeighbor(scout.getFlower(), BeesAlgorithm.getBestSolutionsAmount());
			scout = new Scout(bestNeighbor);
		}
		
	}
	
	private static Flower chooseBestNeighbor(Flower flower, int neigborsAmount) {
		Vector<Flower> neighborhood = new Vector<Flower>();
		for (int i=0;i<neigborsAmount;i++){
			Flower neighbor = flower.choseFlowerNeighbor();
			neighborhood.add(neighbor); 
		}
		Collections.sort(neighborhood);
		return neighborhood.firstElement();
	}
	
	
}
