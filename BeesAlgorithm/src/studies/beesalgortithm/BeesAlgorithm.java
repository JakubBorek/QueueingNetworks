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
	
	// Tworzymy skautów
	scouts = Utilities.createScouts(scoutsAmount); 
	// Wysy³amy skautow na losowe poszukiwania kwiatów 
	Utilities.sendScouts(scouts);
	// Sortujemy skautów pod wzglêdem atrakcyjnoœci kwwiatów, które znaleŸli
	Collections.sort(scouts);
	
	while(!isStopConditionFulfilled()){
	
		//wybieramy elitarnych Skautów - s¹ to Skauci z elitarnymi kwiatami 
		Vector<Scout> scoutsWithEliteFlowers = (Vector<Scout>)scouts.subList(0,elisteSolutionsAmount); 
		//wybieramy najlpeszych Skautów - s¹ to Skauci z najlepszymi kwiatami 
		Vector<Scout> scoutsWithBestFlowers = (Vector<Scout>)scouts.subList(elisteSolutionsAmount, bestSolutionsAmount); 
		//grupujemy pozosta³ych skautów 
		Vector<Scout> restOfScouts = (Vector<Scout>) scouts.subList(bestSolutionsAmount, scoutsAmount);
		// Intensywnie przeszukujemy s¹siedztwo elitarnych kwiatow 
		Utilities.searchEliteFlowersNeigberhood(scoutsWithEliteFlowers); 
		// Mniej intensywnie przeszukujemy s¹siedztwo najlepszych kwiatów 
		Utilities.searchBestFlowersNeigberhood(scoutsWithBestFlowers); 
		// Dla najgorszych kwiatów generujemy nowe
		Utilities.sendScouts(restOfScouts); 
		//Sortujemy wszystkie kwiaty pod wzglêdem atrakcyjnoœci 
		Collections.sort(scouts);
	}
	}
	
	private static boolean isStopConditionFulfilled(){
		//TODO wzi¹Ÿæ posortowany wektor skautów, wzi¹Ÿæ najlepszego skauta i na jego podstawie sprawdziæ czy warunek jest spe³niony
		// zwracamy false - na cele testów
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
