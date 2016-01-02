package studies.beesalgortithm;

import java.util.Collections;
import java.util.Vector;

public class BeesAlgorithm {
	
	private static int scoutsAmount;
	private static int nodesAmount; 
	private static Vector<Scout> scouts;
	private static int processorsMaxAmount;
	private static int elisteSolutionsAmount;
	private static int bestSolutionsAmount;
	
	public BeesAlgorithm(){
	
	// Tworzymy skaut�w
	scouts = Utilities.createScouts(scoutsAmount); 
	// Wysy�amy skautow na losowe poszukiwania kwiat�w 
	Utilities.sendScouts(scouts);
	// Sortujemy skaut�w pod wzgl�dem atrakcyjno�ci kwwiat�w, kt�re znale�li
	Collections.sort(scouts);
	
	while(!isStopConditionFulfilled()){
	
		//wybieramy elitarnych Skaut�w - s� to Skauci z elitarnymi kwiatami 
		Vector<Scout> scoutsWithEliteFlowers = (Vector<Scout>)scouts.subList(0,elisteSolutionsAmount); 
		//wybieramy najlpeszych Skaut�w - s� to Skauci z najlepszymi kwiatami 
		Vector<Scout> scoutsWithBestFlowers = (Vector<Scout>)scouts.subList(elisteSolutionsAmount, bestSolutionsAmount); 
		//grupujemy pozosta�ych skaut�w 
		Vector<Scout> restOfScouts = (Vector<Scout>) scouts.subList(bestSolutionsAmount, scoutsAmount);
		// Intensywnie przeszukujemy s�siedztwo elitarnych kwiatow 
		Utilities.searchEliteFlowersNeigberhood(scoutsWithEliteFlowers); 
		// Mniej intensywnie przeszukujemy s�siedztwo najlepszych kwiat�w 
		Utilities.searchBestFlowersNeigberhood(scoutsWithBestFlowers); 
		// Dla najgorszych kwiat�w generujemy nowe
		Utilities.sendScouts(restOfScouts); 
		//Sortujemy wszystkie kwiaty pod wzgl�dem atrakcyjno�ci 
		Collections.sort(scouts);
	}
	}
	
	private static boolean isStopConditionFulfilled(){
		//TODO wzi��� posortowany wektor skaut�w, wzi��� najlepszego skauta i na jego podstawie sprawdzi� czy warunek jest spe�niony
		// zwracamy false - na cele test�w
		return false;
	}

	public static int getNodesAmount() {
		return nodesAmount;
	}

	public static void setNodesAmount(int nodesAmount) {
		BeesAlgorithm.nodesAmount = nodesAmount;
	}

	public static Vector<Scout> getScouts() {
		return scouts;
	}

	public static void setScouts(Vector<Scout> scouts) {
		BeesAlgorithm.scouts = scouts;
	}

	public static int getProcessorsMaxAmount() {
		return processorsMaxAmount;
	}

	public static void setProcessorsMaxAmount(int processorsMaxAmount) {
		BeesAlgorithm.processorsMaxAmount = processorsMaxAmount;
	}

	public static int getElisteSolutionsAmount() {
		return elisteSolutionsAmount;
	}

	public  void setElisteSolutionsAmount(int elisteSolutionsAmount) {
		this.elisteSolutionsAmount = elisteSolutionsAmount;
	}

	public static int getBestSolutionsAmount() {
		return bestSolutionsAmount;
	}

	public  void setBestSolutionsAmount(int bestSolutionsAmount) {
		this.bestSolutionsAmount = bestSolutionsAmount;
	}
	
}
